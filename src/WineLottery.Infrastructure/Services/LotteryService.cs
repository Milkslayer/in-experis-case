using Microsoft.EntityFrameworkCore;
using WineLottery.Application;
using WineLottery.Application.Interfaces;
using WineLottery.Domain;

namespace WineLottery.Infrastructure.Services;

public class LotteryService(IApplicationDbContext dbContext) : ILotteryService
{
    private readonly IApplicationDbContext _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

    public async Task<IEnumerable<Ticket>> GetTickets(CancellationToken ct = default)
    {
        return await _dbContext.Tickets.OrderBy(x=>x.Number).AsNoTracking().ToListAsync(ct);
    }

    public async Task<IEnumerable<Ticket>> ReserveTickets(string ownerName, int[] numbers, CancellationToken ct = default)
    {
        // Guard against buys in drawing period
        var session = await _dbContext.LotterySessions
            .SingleAsync(ct);
        if (session.Status != LotteryStatus.Open)
            throw new InvalidOperationException("Ticket sales are closed once draw starts.");
        
        if (string.IsNullOrWhiteSpace(ownerName))
            throw new ArgumentException("Owner name must be provided.", nameof(ownerName));

        // Fetch tickets matching requested numbers
        var tickets = await _dbContext.Tickets
            .Where(t => numbers.Contains(t.Number))
            .ToListAsync(ct);

        // Verify all requested tickets exist
        var missing = numbers.Except(tickets.Select(t => t.Number)).ToList();
        if (missing.Count != 0)
            throw new KeyNotFoundException($"Tickets not found: {string.Join(", ", missing)}");

        
        // Reserve and sell each ticket
        foreach (var ticket in tickets)
        {
            ticket.Reserve(ownerName);
            /*  Initiate payment with some payment service
             *  After payment complete the callback will go to hook
             *  Set ticket state to Sold, but for the sake of simplicity and time constraint we will set the state here
             */
            ticket.Sell();
        }

        await _dbContext.SaveChangesAsync(ct);

        return tickets;
    }

    public async Task<IEnumerable<DrawResult>?> InitiateDraw(CancellationToken ct = default)
    {
        // Validate if we have tickets bought
        var soldTicketsCount = await _dbContext.Tickets
            .CountAsync(t => t.Status == TicketStatus.Sold, ct);
        if (soldTicketsCount == 0)
        {
            // No tickets sold yet; nothing to draw
            return null;
        }
        
        // Transition the lottery session to drawing state
        var session = await _dbContext.LotterySessions.SingleAsync(ct);
        session.StartDrawing();
        await _dbContext.SaveChangesAsync(ct);

        // Fetch wines ordered by value (cheapest first)
        var wines = await _dbContext.Wines
            .OrderBy(w => w.Value)
            .ToListAsync(ct);

        // Fetch only tickets that have been sold
        var soldTickets = await _dbContext.Tickets
            .Where(t => t.Status == TicketStatus.Sold)
            .ToListAsync(ct);

        // Shuffle sold tickets
        var random = new Random();
        var shuffledTickets = soldTickets
            .OrderBy(_ => random.Next())
            .ToList();

        var drawCount = Math.Min(wines.Count, shuffledTickets.Count);
        var results = new List<DrawResult>(drawCount);

        for (var i = 0; i < drawCount; i++)
        {
            var wine = wines[i];
            var ticket = shuffledTickets[i];

            ticket.MarkAsDrawn();
            var drawResult = new DrawResult(wine, ticket, DateTime.UtcNow);

            results.Add(drawResult);
        }
        _dbContext.DrawResults.AddRange(results);
        session.Close();
        await _dbContext.SaveChangesAsync(ct);
        return results;
    }
}