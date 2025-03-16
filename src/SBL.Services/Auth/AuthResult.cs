using SBL.Domain.Entities;
using SBL.Domain.Enums;

namespace SBL.Services.Auth;

public class AuthResult
{
    public string UserName { get; set; }
    
    public string Email { get; set; }
    
    public string Token { get; set; }
    
    public Role Role { get; set; }
}
