using Microsoft.AspNetCore.Http;

namespace SBL.Services.Auth;

public class SessionHelper
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SessionHelper(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public void SetUserId(int userId) => _httpContextAccessor.HttpContext?.Session.SetInt32("UserId", userId);

    public int? GetUserId() => _httpContextAccessor.HttpContext?.Session.GetInt32("UserId");

    public void SetUserRole(string role) => _httpContextAccessor.HttpContext?.Session.SetString("UserRole", role);

    public string GetUserRole() => _httpContextAccessor.HttpContext?.Session.GetString("UserRole");

    public void ClearSession()
    {
        _httpContextAccessor.HttpContext?.Session.Clear();
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete(".AspNetCore.Session");
    }
}
