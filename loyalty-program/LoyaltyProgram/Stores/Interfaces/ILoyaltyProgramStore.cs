using LoyaltyProgram.Models;

namespace LoyaltyProgram.Stores.Interfaces;

public interface ILoyaltyProgramStore
{
    Task<LoyaltyProgramUser> GetByIdAsync(int userId);
    Task<bool> DoesUserExist(int userId);
    Task<LoyaltyProgramUser> RegisterUserAsync(LoyaltyProgramUser user);
    Task UpdateUserAsync(LoyaltyProgramUser user);
}