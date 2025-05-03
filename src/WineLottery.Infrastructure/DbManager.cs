using Microsoft.Extensions.DependencyInjection;
using WineLottery.Application;
using WineLottery.Domain;

namespace WineLottery.Infrastructure;

public static class DbManager
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var db = serviceProvider.GetRequiredService<IApplicationDbContext>();

        await SeedTickets(db);
        await SeedWines(db);
        await SeedLotterySessions(db);
        await db.SaveChangesAsync();
        ;
    }

    private static async Task SeedLotterySessions(IApplicationDbContext db)
    {
        if (db.LotterySessions.Any())
            return;

        db.LotterySessions.Add(new LotterySession());
        await db.SaveChangesAsync();
    }

    // Keep wines for simplicity
    public static async Task ClearDatabase(IApplicationDbContext db)
    {
        db.Tickets.RemoveRange(db.Tickets);
        db.Wines.RemoveRange(db.Wines);
        db.DrawResults.RemoveRange(db.DrawResults);
        db.LotterySessions.RemoveRange(db.LotterySessions);
        await db.SaveChangesAsync();
    }

    private static async Task SeedWines(IApplicationDbContext db)
    {
        // Only seed if no wines exist
        if (db.Wines.Any())
            return;

        var wines = new List<Wine>
        {
            new("Pinot Noir", 200m),
            new("Chardonnay", 150m),
            new("Cabernet Sauvignon", 250m),
            new("Merlot", 180m),
            new("Sauvignon Blanc", 130m)
        };

        await db.Wines.AddRangeAsync(wines);
    }

    private static async Task SeedTickets(IApplicationDbContext db)
    {
        if (db.Tickets.Any())
            return;
        
        var tickets = Enumerable.Range(1, 100)
            .Select(num => new Ticket(num))
            .ToList();

        await db.Tickets.AddRangeAsync(tickets);
    }
}