namespace WineLottery.Domain;



public class LotterySession
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public LotteryStatus Status { get; private set; } = LotteryStatus.Open;

    
    public void StartDrawing()
    {
        if (Status != LotteryStatus.Open)
            throw new InvalidOperationException("Can only start drawing when open.");
        Status = LotteryStatus.Drawing;
    }

    public void Close()
    {
        if (Status == LotteryStatus.Closed)
            throw new InvalidOperationException("Already closed.");
        Status = LotteryStatus.Closed;
    }
}