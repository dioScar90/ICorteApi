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
    
    public void UpdateEntity(AddressDtoRequest dto, DateTime? utcNow = null)
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

    public AddressDtoResponse CreateDto() => new(
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
