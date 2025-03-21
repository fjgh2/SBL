using Microsoft.AspNetCore.Identity;
using SBL.Domain.Entities;
using SBL.Domain.Enums;
using SBL.Services.Contracts.Services;

namespace SBL.Services.Auth;

public class AuthService : IAuthService
{
    private readonly TokenHelper _tokenHelper;

    private readonly SessionHelper _sessionHelper;

    private readonly UserManager<User> _userManager;

    private readonly SignInManager<User> _signInManager;

    public AuthService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        SessionHelper sessionHelper,
        TokenHelper tokenHelper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _sessionHelper = sessionHelper;
        _tokenHelper = tokenHelper;
    }

    public async Task<AuthResult> GoogleLoginAsync(string email, string name)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new User
            {
                UserName = name,
                Email = email,
                EmailConfirmed = true,
                SteamId = "",
                PhoneNumber = "",
                PhoneNumberConfirmed = false,
                TwoFactorEnabled = false,
                LockoutEnabled = true,
                AccessFailedCount = 0,
                Role = Role.User,
            };
            var pwd = GeneratePassword();
            var createResult = await _userManager.CreateAsync(user, pwd);
            if (!createResult.Succeeded)
            {
                throw new Exception(
                    $"Failed to create user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
            }

            var roleName = user.Role.ToString();

            var result = await _userManager.AddToRoleAsync(user, roleName);
            if (!result.Succeeded)
            {
                throw new ArgumentException("No such role");
            }
        }

        var token = await _tokenHelper.GenerateToken(user);
        _sessionHelper.SetUserId(user.Id);
        _sessionHelper.SetUserRole(user.Role.ToString());

        await _signInManager.SignInAsync(user, isPersistent: false);

        return new AuthResult
        {
            Email = user.Email,
            UserName = user.UserName,
            Token = token,
            Role = Role.User
        };
    }

    public async Task<AuthResult> LoginAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            throw new InvalidOperationException("User not found");
        }

        var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, true);
        if (!signInResult.Succeeded)
        {
            throw new InvalidOperationException("Invalid login attempt");
        }

        var token = await _tokenHelper.GenerateToken(user);
        _sessionHelper.SetUserId(user.Id);
        _sessionHelper.SetUserRole(user.Role.ToString());
        
        return new AuthResult()
        {
            Token = token,
            Email = user.Email,
            UserName = user.UserName,
            Role = user.Role
        };
    }

    public async Task LogoutAsync()
    {
        _sessionHelper.ClearSession();
        await _signInManager.SignOutAsync();
    }

    public async Task RegisterAsync(User user, string password)
    {
        user.SteamId = "";
        user.PhoneNumber = "";
        user.UserName = user.Email;

        var result = await _userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            throw new Exception();
        }

        var roleResult = await _userManager.AddToRoleAsync(user, user.Role.ToString());
        if (!roleResult.Succeeded)
        {
            throw new Exception(
                $"Failed to assign role: {string.Join(", ", 
                    roleResult.Errors.Select(e => e.Description))}");
        }
    }

    private string GeneratePassword()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()";
        var random = new Random();

        return new string(Enumerable.Repeat(chars, 16)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
