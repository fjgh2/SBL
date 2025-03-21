namespace SBL.Api.Dtos;

public class SuccessfullLoginDto
{
    public string AccessToken { get; set; }
    
    public string RefreshToken { get; set; }
    
    public int ExpiresIn { get; set; }
    
    public string TokenType { get; set; } = "Bearer";
    
    public UserDto User { get; set; }
}
