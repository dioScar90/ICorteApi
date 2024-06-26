using System.ComponentModel.DataAnnotations;
using ICorteApi.Entities;
using ICorteApi.Enums;
using ICorteApi.Validators;

namespace ICorteApi.Dtos;

public record UserDtoResponse(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    UserRole[] Role,
    PersonDtoResponse? PersonDto
) : IDtoResponse;

public record UserDtoRequest(
    int? Id,
    string Email,
    string Password,
    UserRole Role,
    PersonDtoRequest? PersonDto
) : IDtoRequest;

public record UserDtoRegisterRequest(
    string Email,
    string Password,
    string PhoneNumber,
    PersonDtoRequest Person
) : IDtoRequest;

public record UserDtoLoginRequest(
    string Email,
    string Password
) : IDtoRequest;

public record UserDtoForgotPasswordRequest(
    string Email
): IDtoRequest;

public record UserDtoResetPasswordRequest(
    string Email,
    string Token,
    string NewPassword,
    string ConfirmNewPassword
): IDtoRequest;

public record UserDtoUpdateProfileRequest(
    string FirstName,
    string LastName
    // Outros campos que você deseja permitir a atualização.
): IDtoRequest;

public record UserDtoChangePasswordRequest(
    string CurrentPassword,
    string NewPassword,
    string ConfirmNewPassword
): IDtoRequest;

public record UserDtoConfirmEmailRequest(
    string Email,
    string Token
): IDtoRequest;
