using Microsoft.AspNetCore.Mvc;
using SpecialOffers.Models;
using SpecialOffers.Models.Enums;
using SpecialOffers.Stores.Interfaces;

namespace SpecialOffers.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialOffersController : ControllerBase
    {
        private readonly ISpecialOffersStore _specialOffersStore;
        private readonly IEventStore _eventStore;

        public SpecialOffersController(ISpecialOffersStore specialOffersStore, IEventStore eventStore)
        {
            _specialOffersStore = specialOffersStore ?? throw new ArgumentNullException(nameof(specialOffersStore));
            _eventStore = eventStore ?? throw new ArgumentNullException(nameof(eventStore));
        }

        [HttpGet("{id:int}", Name = nameof(GetAsync))]
        public async Task<ActionResult<Offer>> GetAsync(int id)
        {
            var offer = await _specialOffersStore.GetAsync(id);
            if (offer is null)
            {
                return NotFound();
            }

            return Ok(offer);
        }

        [HttpPost]
        public async Task<ActionResult<Offer>> PostAsync([FromBody] Offer offer)
        {
            var newOffer = await _specialOffersStore.CreateOfferAsync(offer);

            _eventStore.RaiseEvent(EventTypes.SpecialOfferCreated, newOffer);

            return CreatedAtRoute(nameof(GetAsync), new { id = newOffer.Id }, newOffer);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutAsync(int id, [FromBody] Offer offer)
        {
            var offerWithId = offer with { Id = id };
            if (!await _specialOffersStore.DoesOfferExist(id))
            {
                return BadRequest();
            }

            var currentOffer = await _specialOffersStore.GetAsync(id);

            await _specialOffersStore.UpdateOfferAsync(offerWithId);

            _eventStore.RaiseEvent(EventTypes.SpecialOfferUpdated, new { OldOffer = currentOffer, NewOffer = offerWithId });

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (!await _specialOffersStore.DoesOfferExist(id))
            {
                return BadRequest();
            }

            var currentOffer = await _specialOffersStore.GetAsync(id);

            await _specialOffersStore.DeleteAsync(id);

            _eventStore.RaiseEvent(EventTypes.SpecialOfferDeleted, new { Offer = currentOffer });

            return Ok();
        }
    }
}
