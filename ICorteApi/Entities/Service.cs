namespace BarberAppApi.Entities;

public class Service : BaseEntity
{
    public int Id { get; set; }
    public string ServiceName { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public TimeSpan Duration { get; set; }

    // Navigation Properties
    public ICollection<Appointment> Appointments { get; set; } = [];
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