using ICorteApi.Entities;
using ICorteApi.Enums;

namespace ICorteApi.Dtos;

public record AddressDtoRequest(
    string Street,
    string Number,
    string? Complement,
    string Neighborhood,
    string City,
    State State,
    string PostalCode,
    string Country
): IDtoRequest;

public record AddressDtoResponse(
    int Id,
    string Street,
    string Number,
    string? Complement,
    string Neighborhood,
    string City,
    State State,
    string PostalCode,
    string Country
) : IDtoResponse;
