using ICorteApi.Domain.Base;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Domain.Entities;

public class Address : BaseEntity
{
    public string Street { get; set; }
    public string Number { get; set; }
    public string? Complement { get; set; }
    public string Neighborhood { get; set; }
    public string City { get; set; }
    public State State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    
    public int BarberShopId { get; set; }
    public BarberShop? BarberShop { get; set; }
}
