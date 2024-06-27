namespace ICorteApi.Entities;

public class BarberShop : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string PhoneNumber { get; set; }
    public string ComercialNumber { get; set; }
    public string ComercialEmail { get; set; }
    public TimeSpan OpeningHours { get; set; }
    public TimeSpan ClosingHours { get; set; }
    public string DaysOpen { get; set; }
    public double Rating { get; set; }
    
    public int OwnerId { get; set; }
    public Person Owner { get; set; }

    public Address Address { get; set; }
    public IEnumerable<Person> Barbers { get; set; } = [];
    public IEnumerable<Schedule> Schedules { get; set; } = [];
    public IEnumerable<Service> Services { get; set; } = [];
    public IEnumerable<Report> Reports { get; set; } = [];
}
