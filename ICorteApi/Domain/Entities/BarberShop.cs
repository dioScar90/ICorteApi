using ICorteApi.Domain.Base;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Domain.Entities;

public class BarberShop : BasePrimaryKeyEntity<int>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string ComercialNumber { get; set; }
    public string ComercialEmail { get; set; }

    public int OwnerId { get; set; }
    public Person Owner { get; set; }

    public Address Address { get; set; }
    public ICollection<RecurringSchedule> RecurringSchedules { get; set; } = [];
    public ICollection<SpecialSchedule> SpecialSchedules { get; set; } = [];
    public ICollection<Person> Barbers { get; set; } = [];
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<Service> Services { get; set; } = [];
    public ICollection<Report> Reports { get; set; } = [];

    // public int Key => Id;
}
