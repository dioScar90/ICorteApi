using ICorteApi.Application.Validators;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Errors;

namespace ICorteApi.Domain.Entities;

public sealed class Address : BaseEntity<Address, AddressDto>
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

    public Address(AddressDto dto, int? barberShopId = null)
    {
        dto.ThrowExceptionIfInvalid(new AddressDtoValidator(), new AddressErrors());
        
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
    
    public override void UpdateEntityByDto(AddressDto dto, DateTime? utcNow = null)
    {
        dto.ThrowExceptionIfInvalid(new AddressDtoValidator(), new AddressErrors());
        
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

    public override AddressDto CreateDto() => new(
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
