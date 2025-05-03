namespace WineLottery.Domain;

public class Wine
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string Name { get; private set; }
    public decimal Value { get; private set; }

    public Wine(string name, decimal value)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("Name is required", nameof(name));
        if (value<0) throw new ArgumentException("Value must be positive", nameof(value));
        
        Name = name;
        Value = value;
    }
}