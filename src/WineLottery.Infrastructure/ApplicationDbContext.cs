using Microsoft.EntityFrameworkCore;
using WineLottery.Application;
using WineLottery.Domain;

namespace WineLottery.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options), IApplicationDbContext
{
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Wine> Wines { get; set; }
    public DbSet<DrawResult> DrawResults { get; set; }
    public DbSet<LotterySession> LotterySessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Ticket>().HasKey(x => x.Id);
        modelBuilder.Entity<Wine>().HasKey(x => x.Id);
        modelBuilder.Entity<DrawResult>().HasKey(x => new {x.TicketId, x.WineId});
        
        base.OnModelCreating(modelBuilder);
    }
}