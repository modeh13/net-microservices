using SpecialOffers.Models;
using SpecialOffers.Stores.Interfaces;

namespace SpecialOffers.Stores
{
    public class EventStore : IEventStore
    {
        private static long _currentSequenceNumber = 0;
        private static readonly IList<EventFeedEvent> Database = new List<EventFeedEvent>();

        public void RaiseEvent(string name, object content)
        {
            var sequenceNumber = Interlocked.Increment(ref _currentSequenceNumber);
            Database.Add(new EventFeedEvent(sequenceNumber, DateTimeOffset.UtcNow, name, content));
        }

        public IEnumerable<EventFeedEvent> GetEvents(long start, long end)
        {
            return Database
                .Where(eventFeed => eventFeed.SequenceNumber >= start && eventFeed.SequenceNumber <= end)
                .OrderBy(eventFeed => eventFeed.SequenceNumber);
        }        
    }
}
