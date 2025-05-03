namespace WineLottery.Domain;

/// <summary>
/// Single lottery ticket with a number and a status of type TicketStatus
/// </summary>
public class Ticket
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public int Number { get; private set; }
    public TicketStatus Status { get; private set; } = TicketStatus.Available;
    public string? OwnerName{ get; private set; }

    public Ticket(int number)
    {
        if (number <= 0) throw new ArgumentOutOfRangeException(nameof(number), "The number must be positive.");
        Number = number;
    }

    public void Reserve(string ownerName)
    {
        if (string.IsNullOrWhiteSpace(ownerName))
            throw new ArgumentException("The owner name can not be empty", nameof(ownerName));
        
        if (Status != TicketStatus.Available)
            throw new InvalidOperationException("Only available status can be reserved.");
        
        OwnerName = ownerName;
        Status = TicketStatus.Reserved;
    }

    public void Sell()
    {
        if (Status != TicketStatus.Reserved)
            throw new InvalidOperationException("Only reserved tickets can be sold.");
        Status = TicketStatus.Sold;
    }

    public void MarkAsDrawn()
    {
        if (Status != TicketStatus.Sold)
            throw new InvalidOperationException("Only sold tickets can be drawn.");
        Status = TicketStatus.Drawn;
    }
}