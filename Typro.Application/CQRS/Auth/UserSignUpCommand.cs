using FluentResults;
using MediatR;
using Typro.Application.Models.User;

namespace Typro.Application.CQRS.Auth;

public record UserSignUpCommand : IRequest<Result<UserGeneralInfoModel>>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}