using WineLottery.Domain;

namespace WineLottery.Application.Interfaces;

// setting up service and repository all in one to save time and simplicity
public interface ILotteryService
{
    Task<IEnumerable<Ticket>> GetTickets(CancellationToken ct = default);

    Task<IEnumerable<Ticket>> ReserveTickets(string ownerName, int[] numbers, CancellationToken ct = default);

    Task<IEnumerable<DrawResult>?> InitiateDraw(CancellationToken ct = default);
    
}