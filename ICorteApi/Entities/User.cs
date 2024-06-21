using System.ComponentModel.DataAnnotations;
using ICorteApi.Enums;
using ICorteApi.Validators;

namespace ICorteApi.Entities;

public class User : BaseEntity
{
    public override int Id { get; set; }
    public string Username { get; set; }
    // private Email _email;
    // public string Email
    // {
    //     get => _email.ToString();
    //     set => _email = new Email(value);
    // }
    // [EmailAddress(ErrorMessage = "Formato de email inválido")]
    [EmailValidator]
    public string Email { get; set; }
    public string Phone { get; set; }
    public Role Role { get; set; }
    public string Password { get; set; }
    
    // Navigation Properties
    public Barber Barber { get; set; }
    // public Client Client { get; set; }
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