using ICorteApi.Application.Services;

namespace ICorteApi.Application.Dtos;

public record UserDto(
    int Id,
    string Email,
    string PhoneNumber,
    string[] Roles,
    ProfileDtoRequest? Profile,
    BarberShopDtoRequest? BarberShop
) : IDto<User>;

public record UserDtoEmailUpdate(
    string Email
) : IDtoRequest<User>;

public record UserDtoPhoneNumberUpdate(
    string PhoneNumber
) : IDtoRequest<User>;

public record UserDtoRegisterRequest(
    string Email,
    string Password,
    ProfileDtoRequest? Profile = null,
    BarberShopDtoRequest? BarberShop = null
) : IDtoRequest<User>;

public record UserDtoLoginRequest(
    string Email,
    string Password
) : IDtoRequest<User>;

public record UserDtoForgotPasswordRequest(
    string Email
) : IDtoRequest<User>;

public record UserDtoPasswordUpdateRequest(
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

public record FoundUserByAdmin(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string PhoneNumber,
    bool IsBarberShop
) : IDtoRequest<User>;
