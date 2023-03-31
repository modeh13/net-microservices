using SpecialOffers.Models;

namespace SpecialOffers.Stores.Interfaces
{
    public interface IEventStore
    {
        void RaiseEvent(string name, object content);
        IEnumerable<EventFeedEvent> GetEvents(long start, long end);
    }
}
