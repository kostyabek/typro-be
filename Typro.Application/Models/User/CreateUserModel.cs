﻿using Typro.Domain.Enums.User;

namespace Typro.Application.Models.User;

public record CreateUserDto(string Email, string PasswordHash, UserRole RoleId);