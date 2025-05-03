using WineLottery.Application;
using WineLottery.Application.Interfaces;
using WineLottery.Domain;
using WineLottery.Infrastructure;

namespace WineLottery.Api;

/// <summary>
/// Contains API endpoint route definitions for the Wine Lottery application.
/// </summary>
public static class Endpoints
{
    /// <summary>
    /// Maps Lottery endpoints
    /// </summary>
    public static void MapEndpoints(this WebApplication app)
    {
        app.MapGet(ApiRoutes.GetLotteryTickets,
                async (ILotteryService lotteryService, CancellationToken ct = default) => await lotteryService.GetTickets(ct))
            .WithName("GetAllTickets")
            .WithTags("Lottery")
            .WithOpenApi()
            .Produces<IEnumerable<Ticket>>();

        app.MapPost(ApiRoutes.ReserveTickets, async (string ownerName, string numbers, ILotteryService lotteryService, CancellationToken ct = default) =>
            {
                if (string.IsNullOrWhiteSpace(ownerName))
                    return Results.BadRequest("OwnerName is required");
                
                if (string.IsNullOrWhiteSpace(numbers))
                    return Results.BadRequest("Numbers are required");

                try
                {
                    var splitNumbers = numbers.Split(",");
                    var parsedNumbers = new HashSet<int>();
                    foreach (var number in splitNumbers)
                    {
                        if (!int.TryParse(number, out var n))
                        {
                            return Results.BadRequest($"Invalid input: {number}");
                        }

                        if (n is < 1 or > 100)
                        {
                            return Results.BadRequest($"Number is required to be in the range 1 - 100: {n}");
                        }
                        parsedNumbers.Add(n);
                    }
                    var result = await lotteryService.ReserveTickets(ownerName, parsedNumbers.ToArray(), ct);
                    return Results.Ok(result);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .WithDescription("Numbers must be between 1 and 100 and comma separated. Example: 1,2,5")
            .WithName("ReserveTickets")
            .WithTags("Lottery")
            .WithOpenApi()
            .Produces(StatusCodes.Status400BadRequest)
            .Produces<Ticket>();

        app.MapPost(ApiRoutes.Draw, async (ILotteryService lotteryService, CancellationToken ct = default) =>
            {
                var result = await lotteryService.InitiateDraw(ct);
                
                if (result is null)
                    return Results.BadRequest("No tickets were bought");
                
                return Results.Ok(result);
            })
            .WithName("DrawWinners")
            .WithTags("Lottery")
            .WithOpenApi()
            .Produces<IEnumerable<DrawResult>>();

        app.MapPost(ApiRoutes.Reset, async (IServiceProvider sp, CancellationToken ct) =>
            {
                try
                {
                    var db = sp.GetRequiredService<IApplicationDbContext>();
                    await DbManager.ClearDatabase(db);
                    await DbManager.Initialize(sp);
                    return Results.Ok();
                }
                catch (Exception e)
                {
                    return Results.InternalServerError(e);
                }
            })
            .WithName("NewLottery")
            .WithTags("Lottery")
            .WithOpenApi();

        app.MapPost(ApiRoutes.PaymentCallBack, () => { })
            .WithName("PaymentCallBack")
            .WithTags("Payment")
            .WithOpenApi();
    }
}