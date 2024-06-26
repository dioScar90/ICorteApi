using System.ComponentModel.DataAnnotations;
using ICorteApi.Entities;
using ICorteApi.Enums;
using ICorteApi.Validators;

namespace ICorteApi.Dtos;

public record PersonDtoRequest(
    string FirstName,
    string LastName,
    AddressDtoRequest[] Addresses
) : IDtoRequest;

public record PersonDtoResponse(
    int Id,
    string FirstName,
    string LastName,
    DateTime? LastVisitDate,
    string[] Roles
    // UserRole[] Roles
    // AddressDtoResponse[] Addresses
) : IDtoResponse;
