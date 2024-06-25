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
    PersonDtoRequest? PersonDto
) : IDtoResponse;

public record UserDtoRequest(
    int? Id,
    string Email,
    string Password,
    UserRole Role,
    PersonDtoRequest? PersonDto
) : IDtoRequest;

public record RegisterDtoRequest(
    string Email,
    string Password,
    string PhoneNumber,
    PersonDtoRequest Person
) : IDtoRequest;

public record LoginDtoRequest(
    string Email,
    string Password
) : IDtoRequest;
