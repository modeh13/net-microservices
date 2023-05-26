namespace ShoppingCart.Abstractions.Stores;

public interface IEventStore
{
    Task<IEnumerable<Event>> GetEventsAsync(long firstEventSequenceNumber, long lastEventSequenceNumber);
    Task RaiseAsync(string eventName, object content);
}
