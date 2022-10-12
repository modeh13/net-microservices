using Microsoft.AspNetCore.Mvc;
using ShoppingCart.EventFeed.Interfaces;

namespace ShoppingCart.EventFeed;

[ApiController]
[Route("api/[controller]")]
public class EventFeedController : ControllerBase
{
    private readonly IEventStore _eventStore;
    
    public EventFeedController(IEventStore eventStore)
    {
        _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
    }

    [HttpGet]
    public Event[] Get([FromQuery] long start, [FromQuery] long end = long.MaxValue)
    {
        return _eventStore.GetEvents(start, end).ToArray();
    }
}