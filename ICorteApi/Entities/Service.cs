using ICorteApi.Enums;

namespace ICorteApi.Entities;

public class Service : BaseEntity
{
    public ServiceType ServiceType { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int BarberId { get; set; }

    // Navigation Properties
    public User Barber { get; set; }
}

/*
CHAT GPT

Service: Representa os serviços oferecidos.

Id (int)
ServiceName (string)
Description (string)
Price (decimal)
Duration (TimeSpan)
CreatedAt (DateTime)
UpdatedAt (DateTime)
IsActive (bool)
Navigation Properties: List<Appointment>
*/