using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Interfaces;

public interface IAppDbContext
{
    DbSet<Person> People { get; set; }
    DbSet<BarberShop> BarberShops { get; set; }
    DbSet<Address> Addresses { get; set; }
    // DbSet<Appointment> Appointments { get; set; }
    // DbSet<Conversation> Conversations { get; set; }
    // DbSet<Message> Messages { get; set; }
    // DbSet<Schedule> Schedules { get; set; }
    // DbSet<RecurringSchedule> RecurringSchedules { get; set; }
    // DbSet<Service> Services { get; set; }
    // DbSet<AppointmentService> AppointmentServices { get; set; }
    // DbSet<ConversationParticipant> ConversationParticipants { get; set; }
    // DbSet<Report> Reports { get; set; }
}
