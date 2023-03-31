using SpecialOffers.Models;
using SpecialOffers.Stores.Interfaces;

namespace SpecialOffers.Stores
{
    public sealed class SpecialOffersStore : ISpecialOffersStore
    {
        private static readonly IDictionary<int, Offer> Database = new Dictionary<int, Offer>();

        private static int GetNextSequenceNumber()
        {
            return Database.Keys.DefaultIfEmpty().Max() + 1;
        }        

        public Task<Offer> GetAsync(int id)
        {
            if (Database.TryGetValue(id, out var offer))
            {
                return Task.FromResult(offer);
            }

            return Task.FromResult<Offer>(default);
        }

        public Task<Offer> CreateOfferAsync(Offer offer)
        {
            var newOffer = offer with { Id = GetNextSequenceNumber() };

            Database.Add(newOffer.Id, newOffer);

            return Task.FromResult(newOffer);
        }

        public Task<bool> DoesOfferExist(int id)
        {
            return Task.FromResult(Database.ContainsKey(id));
        }

        public async Task UpdateOfferAsync(Offer offer)
        {
            if (!await DoesOfferExist(offer.Id))
            {
                return;
            }

            Database[offer.Id] = offer;
        }

        public async Task DeleteAsync(int id)
        {
            if (!await DoesOfferExist(id))
            {
                return;
            }

            Database.Remove(id);
        }
    }
}
