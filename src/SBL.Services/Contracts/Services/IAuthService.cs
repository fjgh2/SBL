using SBL.Domain.Entities;
using SBL.Services.Auth;

namespace SBL.Services.Contracts.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(string accessToken, string email, string name);

    Task<AuthResponse> LoginAsync(string email, string password);

    Task RegisterAsync(User user, string password);

    Task LogoutAsync();
}
