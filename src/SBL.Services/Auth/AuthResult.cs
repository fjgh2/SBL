using SBL.Domain.Enums;

namespace SBL.Services.Auth;

public class AuthResult
{
    public string Token { get; set; }
    
    public Role Role { get; set; }
}
