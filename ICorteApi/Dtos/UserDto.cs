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
    string PhoneNumber,
    UserRole Role,
    IEnumerable<AddressDtoResponse> Addresses
) : IDtoResponse;

public record UserDtoRequest(
    int? Id,
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string PhoneNumber,
    UserRole Role,
    IEnumerable<AddressDtoRequest> Addresses
) : IDtoRequest;

public record UserLoginDtoRequest(
    string Email,
    string Password,
    IEnumerable<AddressDtoRequest> Addresses
) : IDtoRequest;
