using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Domain.Entities;

public sealed class Address : BasePrimaryKeyEntity<Address, int>
{
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string? Complement { get; private set; }
    public string Neighborhood { get; private set; }
    public string City { get; private set; }
    public State State { get; private set; }
    public string PostalCode { get; private set; }
    public string Country { get; private set; }

    public int BarberShopId { get; init; }
    public BarberShop? BarberShop { get; init; }

    private Address() {}

    public Address(AddressDtoRequest dto, int? barberShopId = null)
    {
        Street = dto.Street;
        Number = dto.Number;
        Complement = dto.Complement;
        Neighborhood = dto.Neighborhood;
        City = dto.City;
        State = dto.State;
        PostalCode = dto.PostalCode;
        Country = dto.Country;

        BarberShopId = barberShopId ?? default;
    }

    private void UpdateByAddressDto(AddressDtoRequest dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        Street = dto.Street;
        Number = dto.Number;
        Complement = dto.Complement;
        Neighborhood = dto.Neighborhood;
        City = dto.City;
        State = dto.State;
        PostalCode = dto.PostalCode;
        Country = dto.Country;

        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<Address> requestDto, DateTime? utcNow = null)
    {
        if (requestDto is AddressDtoRequest dto)
            UpdateByAddressDto(dto, utcNow);
            
        throw new Exception("Dados enviados inválidos");
    }
}
