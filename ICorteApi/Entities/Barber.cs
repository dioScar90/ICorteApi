namespace ICorteApi.Entities;

public class Barber : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string PhoneNumber { get; set; }
    public string ComercialNumber { get; set; }
    public string ComercialEmail { get; set; }
    public TimeSpan OpeningHours { get; set; }
    public TimeSpan ClosingHours { get; set; }
    public string DaysOpen { get; set; }
    // public string Website { get; set; }
    // public string SocialMediaLinks { get; set; }
    public string ServicesOffered { get; set; }
    public double Rating { get; set; }

    public int PersonId { get; set; }
    public Person Person { get; set; }

    public Address Address { get; set; }

    public IEnumerable<Schedule> Schedules { get; set; } = [];
}
