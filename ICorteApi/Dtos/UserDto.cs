using System.ComponentModel.DataAnnotations;
using ICorteApi.Entities;
using ICorteApi.Enums;
using ICorteApi.Validators;

namespace ICorteApi.Dtos;

public record UserDtoResponse(
    int Id,
    string Name,
    string Email,
    string PhoneNumber,
    UserRole Role,
    IEnumerable<AddressDtoResponse> Addresses
) : IDtoResponse;

public record UserDtoRequest(
    int? Id,
    string Name,
    string Email,
    string Password,
    string PhoneNumber,
    UserRole Role,
    IEnumerable<AddressDtoRequest> Addresses
) : IDtoRequest;
