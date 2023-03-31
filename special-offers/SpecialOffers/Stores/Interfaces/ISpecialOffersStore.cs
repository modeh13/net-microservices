using SpecialOffers.Models;

namespace SpecialOffers.Stores.Interfaces
{
    public interface ISpecialOffersStore
    {
        Task<Offer> GetAsync(int id);
        Task<Offer> CreateOfferAsync(Offer offer);
        Task<bool> DoesOfferExist(int id);
        Task UpdateOfferAsync(Offer offer);
        Task DeleteAsync(int id);
    }
}
