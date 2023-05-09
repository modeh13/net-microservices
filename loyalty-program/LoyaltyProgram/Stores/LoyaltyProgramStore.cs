using LoyaltyProgram.Models;
using LoyaltyProgram.Stores.Interfaces;

namespace LoyaltyProgram.Stores;

public class LoyaltyProgramStore : ILoyaltyProgramStore
{
    private static readonly IDictionary<int, LoyaltyProgramUser> Database = new Dictionary<int, LoyaltyProgramUser>();

    private static int NextSequenceNumber()
    {
        return Database.Keys.DefaultIfEmpty().Max() + 1;
    }

    public Task<LoyaltyProgramUser> GetByIdAsync(int userId)
    {
        return Database.TryGetValue(userId, out var loyaltyProgramUser) ? Task.FromResult(loyaltyProgramUser) : default;
    }

    public Task<bool> DoesUserExist(int userId)
    {
        return Task.FromResult(Database.ContainsKey(userId));
    }

    public Task<LoyaltyProgramUser> RegisterUserAsync(LoyaltyProgramUser user)
    {
        var nextSequenceNumber = NextSequenceNumber();
        var newUser = user with { Id = nextSequenceNumber };
        
        Database.Add(nextSequenceNumber, newUser);

        return Task.FromResult(Database[nextSequenceNumber]);
    }

    public async Task UpdateUserAsync(LoyaltyProgramUser user)
    {
        if (!await DoesUserExist(user.Id))
        {
            return;
        }

        Database[user.Id] = user;
    }
}
