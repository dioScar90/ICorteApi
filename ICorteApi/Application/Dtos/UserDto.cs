using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Application.Dtos;

public record PersonDtoRequest(
    string FirstName,
    string LastName
) : IDtoRequest<User>;

public record PersonDtoResponse(
    int UserId,
    string FirstName,
    string LastName,
    BarberShopDtoResponse? OwnedBarberShop
) : IDtoResponse<User>;


public record UserDtoRequest(
    string Email,
    string Password,
    UserRole Role,
    PersonDtoRequest? PersonDto
) : IDtoRequest<User>;

public record UserDtoResponse(
    int Id,
    // string FirstName,
    // string LastName,
    string Email,
    // UserRole[] Roles,
    string[] Roles,
    PersonDtoResponse? PersonDto
) : IDtoResponse<User>;

public record UserDtoRegisterRequest(
    string Email,
    string Password,
    string PhoneNumber,
    PersonDtoRequest PersonDto
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
