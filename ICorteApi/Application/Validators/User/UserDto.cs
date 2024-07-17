// using ICorteApi.Application.Interfaces;
// using ICorteApi.Domain.Enums;

// namespace ICorteApi.Application.Dtos;

// public record UserDtoRequest(
//     string Email,
//     string Password,
//     UserRole Role,
//     PersonDtoRequest? PersonDto
// ) : IDtoRequest;

// public record UserDtoResponse(
//     int Id,
//     // string FirstName,
//     // string LastName,
//     string Email,
//     // UserRole[] Roles,
//     string[] Roles,
//     PersonDtoResponse? PersonDto
// ) : IDtoResponse;

// public record UserDtoRegisterRequest(
//     string Email,
//     string Password,
//     string PhoneNumber,
//     PersonDtoRequest PersonDto
// ) : IDtoRequest;

// public record UserDtoLoginRequest(
//     string Email,
//     string Password
// ) : IDtoRequest;

// public record UserDtoForgotPasswordRequest(
//     string Email
// ) : IDtoRequest;

// public record UserDtoResetPasswordRequest(
//     string Email,
//     string Token,
//     string NewPassword,
//     string ConfirmNewPassword
// ) : IDtoRequest;

// public record UserDtoUpdateProfileRequest(
//     string FirstName,
//     string LastName
// ) : IDtoRequest;

// public record UserDtoChangePasswordRequest(
//     string CurrentPassword,
//     string NewPassword,
//     string ConfirmNewPassword
// ) : IDtoRequest;

// public record UserDtoConfirmEmailRequest(
//     string Email,
//     string Token
// ) : IDtoRequest;
