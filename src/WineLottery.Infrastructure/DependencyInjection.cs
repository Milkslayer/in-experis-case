using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WineLottery.Application;
using WineLottery.Application.Interfaces;
using WineLottery.Domain;
using WineLottery.Infrastructure.Services;

namespace WineLottery.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(opts => opts.UseInMemoryDatabase("WineLottery"));
        builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());
        builder.Services.AddScoped<ILotteryService, LotteryService>();
        
        
    }
}