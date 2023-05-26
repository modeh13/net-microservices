namespace ShoppingCart.Models;

public record Event
{
    public long SequenceNumber { get; init; }
    public string? Name { get; init; } 
    public object? Content { get; init; }
    public DateTimeOffset OccurredAt { get; init; }
}
