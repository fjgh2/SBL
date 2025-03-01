using SBL.Domain.Enums;

namespace SBL.Services.Auth;

public class AuthResponse
{
    public string Token { get; set; }
    
    public Role Role { get; set; }
}
