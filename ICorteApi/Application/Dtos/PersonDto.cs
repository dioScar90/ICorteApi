using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Dtos;

public record PersonDtoRegisterRequest(
    string FirstName,
    string LastName,
    Gender Gender,
    string PhoneNumber,
    string? ImageUrl = null
) : IDtoRequest<Person>;

public record PersonDtoRequest(
    string FirstName,
    string LastName,
    Gender Gender,
    string PhoneNumber,
    string? ImageUrl = null
) : IDtoRequest<Person>;

public record PersonDtoResponse(
    int Id,
    string FirstName,
    string LastName,
    Gender Gender,
    string? ImageUrl
) : IDtoResponse<Person>;
