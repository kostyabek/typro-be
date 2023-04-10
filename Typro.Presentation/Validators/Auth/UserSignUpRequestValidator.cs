using System.Text.RegularExpressions;
using FluentValidation;
using Typro.Presentation.Models.Request.Auth;

namespace Typro.Presentation.Validators.Auth;

public partial class UserSignUpRequestValidator : AbstractValidator<UserSignUpRequest>
{
    public UserSignUpRequestValidator()
    {
        RuleFor(e => e.Email)
            .Matches(EmailRegex())
            .WithMessage("Invalid e-mail format");
        RuleFor(e => e.Password)
            .Matches(PasswordRegex())
            .WithMessage("Password must consist of 8 or more characters and include 1 special character, 1 number, 1 upper and lowercase letters");
        RuleFor(e => e.ConfirmPassword)
            .Equal(e => e.Password)
            .WithMessage("Passwords do not match");
    }

    [GeneratedRegex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]
    private static partial Regex EmailRegex();

    [GeneratedRegex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$")]
    private static partial Regex PasswordRegex();
}