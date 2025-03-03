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
    public async Task<ActionResult<AuthResponse>> GoogleLogin(GoogleLoginRequest request)
    {
        if (request == null || string.IsNullOrEmpty(request.AccessToken) || 
            string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Name))
        {
            return BadRequest(new ProblemDetails { Title = "Invalid request data" });
        }

        var result = await _authService.LoginAsync(request.AccessToken, request.Email, request.Name);
        
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<User>> LoginAsync(LoginDto loginDto)
    {
        if (loginDto == null)
        {
            return BadRequest(new ProblemDetails() { Title = $"Invalid user data" });
        }

        var result = await _authService.LoginAsync(loginDto.Email, loginDto.Password);

        return Ok(result);
    }

    [Authorize(Roles = "AuthCustomer,Admin")]
    [HttpPost("logout")]
    public async Task<ActionResult> LogoutAsync(LogoutDto dto)
    {
        await _authService.LogoutAsync();
        
        return Ok(new { message = "Logged out successfully" });
    }

    [HttpPost("register")]
    public async Task<ActionResult<int>> RegisterAsync(RegisterDto registerDto)
    {
        if (registerDto == null)
        {
            return BadRequest(new ProblemDetails { Title = "Invalid register data" });
        }

        var user = _mapper.Map<User>(registerDto);
        user.Role = Role.User;
        await _authService.RegisterAsync(user, registerDto.Password);

        return CreatedAtAction("GetUser", "User", new { id = user.Id }, user.Id);
    }
}
