namespace ICorteApi.Application.Dtos;

public record AddressDto(
    int Id,
    int BarberShopId,
    string Street,
    string Number,
    string? Complement,
    string Neighborhood,
    string City,
    State State,
    string PostalCode,
    string Country
) : IDto<Address>;
