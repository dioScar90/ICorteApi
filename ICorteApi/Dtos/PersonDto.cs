using System.ComponentModel.DataAnnotations;
using ICorteApi.Entities;
using ICorteApi.Enums;
using ICorteApi.Validators;

namespace ICorteApi.Dtos;

public record PersonDtoResponse(
    int Id,
    string FirstName,
    string LastName,
    UserRole[] Roles,
    AddressDtoResponse[] Addresses
) : IDtoResponse;

public record PersonDtoRequest(
    int? Id,
    string FirstName,
    string LastName,
    AddressDtoRequest[] Addresses
) : IDtoRequest;
