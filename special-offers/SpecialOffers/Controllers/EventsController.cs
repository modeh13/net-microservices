using Microsoft.AspNetCore.Mvc;
using SpecialOffers.Models;
using SpecialOffers.Stores.Interfaces;

namespace SpecialOffers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventStore _eventStore;

        public EventsController(IEventStore eventStore) 
        { 
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        [HttpGet]
        public async Task<ActionResult<EventFeedEvent>> GetAsync([FromQuery] long start, [FromQuery] long end)
        {
            if (start < 0 || end < start)
            { 
                return BadRequest();
            }

            var eventFeedEvents = await Task.FromResult(_eventStore.GetEvents(start, end).ToArray());

            return Ok(eventFeedEvents);
        }
    }
}
