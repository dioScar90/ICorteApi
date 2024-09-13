using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public sealed class Address : BaseEntity<Address>
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

    private Address() { }

    public Address(AddressDtoCreate dto, int? barberShopId = null)
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

    private void UpdateByAddressDto(AddressDtoUpdate dto, DateTime? utcNow)
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
        switch (requestDto)
        {
            case AddressDtoUpdate dto:
                UpdateByAddressDto(dto, utcNow);
                break;
            default:
                throw new Exception("Dados enviados inválidos");
        }
    }

    public override AddressDtoResponse CreateDto() =>
        new(
            Id,
            BarberShopId,
            Street,
            Number,
            Complement,
            Neighborhood,
            City,
            State,
            PostalCode,
            Country
        );
}

public enum State
{
    AC, AL, AP, AM, BA, CE, DF, ES, GO, MA, MT, MS, MG, PA, PB, PR, PE, PI, RJ, RN, RS, RO, RR, SC, SP, SE, TO
}
