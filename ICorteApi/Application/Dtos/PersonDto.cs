using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Dtos;

public record PersonDtoRequest(
    string FirstName,
    string LastName
) : IDtoRequest<Person>;

public record PersonDtoResponse(
    int UserId,
    string FirstName,
    string LastName,
    BarberShopDtoResponse? OwnedBarberShop
) : IDtoResponse<Person>;
