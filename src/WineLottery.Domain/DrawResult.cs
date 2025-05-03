namespace WineLottery.Domain;

public class DrawResult
{
    public Guid Id { get; set; } = Guid.NewGuid();
     public Guid TicketId { get; private set; }
     public Ticket Ticket { get; private set; } = default!;
    public Guid WineId { get; private set; }
    public Wine Wine { get; private set; } = default!;
    public DateTime DrawnAt { get; }

    public DrawResult()
    {
    }

    public DrawResult(Wine wine, Ticket ticket, DateTime drawnAt)
    {
        Wine = wine;
        Ticket = ticket;
        DrawnAt = drawnAt;
    }
}