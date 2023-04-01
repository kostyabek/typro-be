using System.Text.RegularExpressions;
using FluentValidation;
using Typro.Presentation.Models.Request.Auth;

namespace Typro.Presentation.Validators.Auth;

public partial class UserSignInRequestValidator : AbstractValidator<UserSignInRequest>
{
    public UserSignInRequestValidator()
    {
        RuleFor(e => e.Email)
            .Matches(EmailRegex());
    }

    [GeneratedRegex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$")]
    private static partial Regex EmailRegex();
}