using SBL.Domain.Entities;
using SBL.Services.Auth;

namespace SBL.Services.Contracts.Services;

public interface IAuthService
{
    Task<AuthResult> GoogleLoginAsync(string email, string name);

    Task<AuthResult> LoginAsync(string email, string password);

    Task RegisterAsync(User user, string password);

    Task LogoutAsync();
}
