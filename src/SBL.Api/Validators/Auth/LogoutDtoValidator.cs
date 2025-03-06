using FluentValidation;
using SBL.Api.Dtos;

namespace SBL.Api.Validators.Auth;

public class LogoutDtoValidator : AbstractValidator<LogoutDto>
{
    public LogoutDtoValidator()
    {
        RuleFor(x => x.AccessToken)
            .NotEmpty()
            .WithMessage("Access token is required.");
            
        RuleFor(x => x.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
}
