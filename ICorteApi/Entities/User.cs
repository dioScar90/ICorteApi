using System.ComponentModel.DataAnnotations;
using ICorteApi.Enums;
using ICorteApi.Validators;

namespace ICorteApi.Entities;

public class User : BaseUser
{
    public string Name { get; set; }
    [EmailValidator]
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime? LastVisitDate { get; set; }
    public UserRole Role { get; set; }
    
    public IEnumerable<Appointment> Appointments { get; set; }
    public IEnumerable<Conversation> Conversations { get; set; }
    public IEnumerable<Schedule> Schedules { get; set; }
    public IEnumerable<Service> Services { get; set; }
    public IEnumerable<Report> Reports { get; set; }
    public IEnumerable<Address> Addresses { get; set; }
}

/*
CHAT GPT

User: Representa todos os usuários do sistema (administradores, barbers, clients, etc.).

Id (int)
Name (string)
Email (string)
Phone (string)
Role (enum - Admin, Barber, Client, etc.)
PasswordHash (string)
CreatedAt (DateTime)
UpdatedAt (DateTime)
IsActive (bool)
Navigation Properties: List<Barber>, List<Client>
*/