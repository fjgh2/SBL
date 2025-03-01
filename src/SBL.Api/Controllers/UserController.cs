using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBL.Domain.Entities;
using SBL.Domain.Enums;
using SBL.Services.Contracts.Services;

namespace SBL.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    // [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<List<User>>> GetUsersAsync()
    {
        var users = await _userService.GetAllUsersAsync();
        if (users == null)
        {
            return NotFound();
        }

        return Ok(users.ToArray());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<User>> GetUserAsync(int id)
    {
        if (id < 1)
        {
            return BadRequest(new ProblemDetails() {Title = "Invalid user id."});
        }
        
        var user = await _userService.GetUserAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        return Ok(user);
    }
    
    // [Authorize(Roles = "Admin")]
    [HttpPut("{id:int}/role")]
    public async Task<IActionResult> UpdateUserRoleAsync(int id, [FromBody] Role role)
    {
        if (id < 1)
        {
            return BadRequest(new ProblemDetails { Title = "Invalid user id." });
        }
    
        try
        {
            await _userService.UpdateUserRoleAsync(id, role);
            
            return Ok();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new ProblemDetails { Title = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ProblemDetails { Title = ex.Message });
        }
    }
}
