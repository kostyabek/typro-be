using FluentResults;
using MediatR;
using Typro.Application.Models.User;

namespace Typro.Application.CQRS.Auth.UserSignUp;

public record UserSignUpCommand
    (string Email, string Password, string ConfirmPassword) : IRequest<Result<UserGeneralInfoModel>>;