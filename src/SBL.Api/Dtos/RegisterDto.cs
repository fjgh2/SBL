using SBL.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace SBL.Api.Dtos;

public class RegisterDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string UserName { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; }

    public Role Role { get; set; } = Role.User;
}

