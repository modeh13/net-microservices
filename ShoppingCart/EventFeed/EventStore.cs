using ShoppingCart.EventFeed.Interfaces;

namespace ShoppingCart.EventFeed;

public class EventStore : IEventStore
{
    private sealed class EventStoreDatabase
    {
        private readonly IDictionary<long, Event> Events;

        public EventStoreDatabase()
        {
            Events = new Dictionary<long, Event>();
        }
        
        public long NextSequenceNumber()
        {
            return Events.Keys.Max() + 1;
        }

        public void Add(Event @event)
        {
            Events.Add(@event.SequenceNumber, @event);
        }

        public IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
        {
            return Events.Values
                .Where(evt => evt.SequenceNumber >= firstEventSequenceNumber && evt.SequenceNumber <= lastEventSequenceNumber)
                .OrderBy(evt => evt.SequenceNumber);
        }
    }

    private static readonly EventStoreDatabase Database = new();

    public IEnumerable<Event> GetEvents(long firstEventSequenceNumber, long lastEventSequenceNumber)
    {
        return Database.GetEvents(firstEventSequenceNumber, lastEventSequenceNumber);
    }

    public void Raise(string eventName, object content)
    {
        var sequenceNumber = Database.NextSequenceNumber();
        
        Database.Add(new Event(sequenceNumber, DateTimeOffset.UtcNow, eventName, content));
    }
}