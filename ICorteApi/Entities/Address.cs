using ICorteApi.Enums;

namespace ICorteApi.Entities;

public class Address : BaseEntity
{
    public string Street { get; set; }
    public string Number { get; set; }
    public string? Complement { get; set; }
    public string Neighborhood { get; set; }
    public string City { get; set; }
    public State State { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; } = "Brasil";
    public AddressType AddressType { get; set; }
    
    public int BarberId { get; set; }
    public Barber Barber { get; set; }
}

/*
CHAT GPT

Address: Representa os endereços.

Id (int)
ClientId (int) - Foreign Key
Street (string)
City (string)
State (string)
PostalCode (string)
Country (string)
Navigation Properties: Client
*/