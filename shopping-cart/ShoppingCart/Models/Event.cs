namespace ShoppingCart.Models;

public record Event(long SequenceNumber, DateTimeOffset OccurredAt, string Name, object Content);
