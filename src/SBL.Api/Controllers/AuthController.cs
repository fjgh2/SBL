using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SBL.Api.Dtos;
using SBL.Domain.Entities;
using SBL.Domain.Enums;
using SBL.Services.Auth;
using SBL.Services.Contracts.Services;

namespace SBL.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    private readonly IMapper _mapper;

    public AuthController(IAuthService authService, IMapper mapper)
    {
        _authService = authService;
        _mapper = mapper;
    }

    [HttpPost("google-login")]
    public async Task<ActionResult<AuthResult>> GoogleLogin(GoogleLoginRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.Email))
        {
            return BadRequest(new ProblemDetails { Title = "Invalid request data" });
        }

        var result = await _authService.GoogleLoginAsync(request.Email, request.Name);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> Login(LoginDto loginDto)
    {
        if (loginDto == null)
        {
            return BadRequest(new ProblemDetails() { Title = $"Invalid user data" });
        }

        var result = await _authService.LoginAsync(loginDto.Email, loginDto.Password);

        return Ok(result);
    }

    // [Authorize(Roles = "User,Moderator,Admin")]
    [HttpPost("logout")]
    public async Task<ActionResult> Logout([FromServices] IHttpContextAccessor contextAccessor)
    {
        var currUser = User.Identity?.Name;
        var userClaims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        await _authService.LogoutAsync();

        return Ok();
    }

    [HttpPost("register")]
    public async Task<ActionResult<int>> Register(RegisterDto registerDto)
    {
        if (registerDto == null)
        {
            return BadRequest(new ProblemDetails { Title = "Invalid register data" });
        }

        var user = _mapper.Map<User>(registerDto);
        user.Role = Role.User;
        await _authService.RegisterAsync(user, registerDto.Password);

        return Ok();
    }
}
