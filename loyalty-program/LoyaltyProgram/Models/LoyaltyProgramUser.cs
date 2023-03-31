namespace LoyaltyProgram.Models;

public record LoyaltyProgramUser(int Id,
    string Name,
    int LoyaltyPoints,
    LoyaltyProgramSettings Settings);

public record LoyaltyProgramSettings
{
    public LoyaltyProgramSettings(string[] interests)
    {
        Interests = interests;
    }

    public string[] Interests { get; init; } = Array.Empty<string>();
}    