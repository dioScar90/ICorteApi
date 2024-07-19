using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record PersonDtoRequest(
    string FirstName,
    string LastName
) : IDtoRequest;

public record PersonDtoResponse(
    int UserId,
    string FirstName,
    string LastName,
    BarberShopDtoResponse? OwnedBarberShop
) : IDtoResponse;
