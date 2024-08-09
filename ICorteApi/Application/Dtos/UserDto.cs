using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Application.Dtos;

public record UserDtoRequest(
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? ImageUrl,
    UserRole[] Roles
) : IDtoRequest<User>;

public record UserDtoResponse(
    int Id,
    string Email,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string? ImageUrl,
    UserRole[] Roles
    // ,
    // bool IsRegisterCompleted
) : IDtoResponse<User>;

public record UserDtoRegisterRequest(
    string Email,
    string Password
) : IDtoRequest<User>;

public record UserDtoLoginRequest(
    string Email,
    string Password
) : IDtoRequest<User>;

public record UserDtoForgotPasswordRequest(
    string Email
) : IDtoRequest<User>;

public record UserDtoResetPasswordRequest(
    string Email,
    string Token,
    string NewPassword,
    string ConfirmNewPassword
) : IDtoRequest<User>;

public record UserDtoUpdateProfileRequest(
    string FirstName,
    string LastName
) : IDtoRequest<User>;

public record UserDtoChangePasswordRequest(
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword
) : IDtoRequest<User>;

public record UserDtoConfirmEmailRequest(
    string Email,
    string Token
) : IDtoRequest<User>;
