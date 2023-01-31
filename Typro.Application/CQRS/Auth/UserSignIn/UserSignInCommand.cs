using FluentResults;
using MediatR;
using Typro.Application.Models.User;

namespace Typro.Application.CQRS.Auth.UserSignIn;

public record UserSignInCommand(string Email, string Password) : IRequest<Result<UserGeneralInfoModel>>;