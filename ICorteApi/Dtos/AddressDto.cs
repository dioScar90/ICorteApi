using BarberAppApi.Enums;

namespace BarberAppApi.Dtos;

public record AddressDto(
    StreetType StreetType,
    string Street,
    string Number,
    string? Complement,
    string Neighborhood,
    string City,
    State State,
    string PostalCode,
    string Country
);
