namespace WineLottery.Api;

/// <summary>
/// Defines available endpoints for the API 
/// </summary>
public static class ApiRoutes
{
    private const string Base = "/api/v1";

    /// <summary>
    /// Returns a list of tickets with their current state
    /// </summary>
    public const string GetLotteryTickets = Base + "/lottery/tickets";
    
    /// <summary>
    /// Reserve the tickets. This endpoint will handle everything from reserve->sold to siplify
    /// </summary>
    public const string ReserveTickets = Base + "/lottery/tickets/reserve";
    
    
    /// <summary>
    /// Reserve the tickets. This endpoint will handle everything from reserve->sold to siplify
    /// </summary>
    public const string Draw = Base + "/lottery/draw";
    
    /// <summary>
    /// Resets the application state for simplicity. Should have been Create new Lottery, but storing things in ram will
    /// kill the service
    /// </summary>
    public const string Reset = Base + "/reset";
    
    
    /// <summary>
    /// Mock hook to handle payment from payment provider
    /// </summary>
    public const string PaymentCallBack = Base + "/payment-hook";
    
}