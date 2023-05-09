using LoyaltyProgram.Models;
using LoyaltyProgram.Stores.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LoyaltyProgram.Controllers;

// TODO: GR - It's missing to implement an API Gateway to call LoyaltyProgram microservice (Commands and Queries collaboration).

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILoyaltyProgramStore _loyaltyProgramStore;

    public UsersController(ILoyaltyProgramStore loyaltyProgramStore)
    {
        _loyaltyProgramStore = loyaltyProgramStore ?? throw new ArgumentNullException(nameof(loyaltyProgramStore));
    }
    
    [HttpGet("{userId:int}")]
    public async Task<ActionResult<LoyaltyProgramUser>> GetAsync(int userId)
    {
        var user = await _loyaltyProgramStore.GetByIdAsync(userId);
        if (user is null)
        {
            return NotFound();
        }

        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<LoyaltyProgramUser>> PostAsync([FromBody] LoyaltyProgramUser user)
    {
        if (user is null)
        {
            return BadRequest();
        }

        var newUser = await _loyaltyProgramStore.RegisterUserAsync(user);

        return Created(new Uri($"/api/Users/{newUser.Id}", UriKind.Relative), newUser);
    }

    [HttpPut("userId:int")]
    public async Task<IActionResult> PutAsync(int userId, [FromBody] LoyaltyProgramUser user)
    {
        if (!await _loyaltyProgramStore.DoesUserExist(userId))
        {
            return BadRequest();
        }

        await _loyaltyProgramStore.UpdateUserAsync(user);

        return NoContent();
    }
}
