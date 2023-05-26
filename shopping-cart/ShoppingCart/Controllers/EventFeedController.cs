using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Abstractions.Stores;

namespace ShoppingCart.Controllers;

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
    public async Task<ActionResult<Event[]>> GetAsync([FromQuery] long start, [FromQuery] long end = long.MaxValue)
    {
        var events = await _eventStore.GetEventsAsync(start, end);
        
        return Ok(events.ToArray());
    }
}
