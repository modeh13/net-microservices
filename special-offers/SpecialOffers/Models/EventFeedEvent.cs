namespace SpecialOffers.Models;

public record EventFeedEvent(long SequenceNumber, DateTimeOffset OccurredAt, string Name, object Content);
