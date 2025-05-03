using Microsoft.EntityFrameworkCore;
using WineLottery.Domain;

namespace WineLottery.Application;

public interface IApplicationDbContext
{
    DbSet<Ticket> Tickets { get; }
    DbSet<Wine> Wines { get; }
    DbSet<DrawResult> DrawResults { get; }
    
    DbSet<LotterySession> LotterySessions { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}