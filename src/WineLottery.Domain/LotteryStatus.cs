namespace WineLottery.Domain;

public enum LotteryStatus
{
    Open,       // reservation and sale is possible
    Drawing,    // blocking purchases; drawing winners
    Closed      // fully finished
}