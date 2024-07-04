using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class BarberShop : BaseEntity
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string ComercialNumber { get; set; }
    public string ComercialEmail { get; set; }
    public double Rating { get; set; }

    public int OwnerId { get; set; }
    public Person Owner { get; set; }

    public Address Address { get; set; }
    public ICollection<OperatingSchedule> OperatingSchedules { get; set; } = [];
    public ICollection<Person> Barbers { get; set; } = [];
    // public ICollection<Schedule> Schedules { get; set; } = [];
    // public ICollection<Service> Services { get; set; } = [];
    // public ICollection<Report> Reports { get; set; } = [];
}
