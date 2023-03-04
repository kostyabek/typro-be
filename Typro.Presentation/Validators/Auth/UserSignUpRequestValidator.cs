using System.Text.RegularExpressions;
using FluentValidation;
using Typro.Presentation.Models.Request.Auth;

namespace Typro.Presentation.Validators.Auth;

public partial class UserSignUpRequestValidator : AbstractValidator<UserSignUpRequest>
{
    public UserSignUpRequestValidator()
    {
        RuleFor(e => e.Email)
            .Matches(EmailRegex());
        RuleFor(e => e.Password)
            .Matches(PasswordRegex());
        RuleFor(e => e.ConfirmPassword)
            .Equal(e => e.Password);
    }

    [GeneratedRegex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]
    private static partial Regex EmailRegex();

    [GeneratedRegex("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{8,}$")]
    private static partial Regex PasswordRegex();
}