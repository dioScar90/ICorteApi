using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Dtos;

public record UserDtoResponse(
    int Id,
    string Email,
    string PhoneNumber,
    string[] Roles,
    ProfileDtoResponse? Profile,
    BarberShopDtoResponse? BarberShop
) : IDtoResponse<User>;

public record UserDtoEmailUpdate(
    string Email
) : IDtoRequest<User>;

public record UserDtoPhoneNumberUpdate(
    string PhoneNumber
) : IDtoRequest<User>;

public record UserDtoRegisterCreate(
    string Email,
    string Password,
    ProfileDtoCreate? Profile = null,
    BarberShopDtoCreate? BarberShop = null
) : IDtoRequest<User>;

public record UserDtoLoginRequest(
    string Email,
    string Password
) : IDtoRequest<User>;

public record UserDtoForgotPasswordRequest(
    string Email
) : IDtoRequest<User>;

public record UserDtoPasswordUpdate(
    string CurrentPassword,
    string NewPassword
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

public record UserDtoConfirmEmailRequest(
    string Email,
    string Token
) : IDtoRequest<User>;
