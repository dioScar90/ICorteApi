using ICorteApi.Application.Validators;
using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public class User : BasePrimaryKeyUserEntity<int>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? ImageUrl { get; set; }

    public bool IsRegisterCompleted { get; set; } = false;
    
    public int? BarberShopId { get; set; }
    public BarberShop BarberShop { get; set; }

    public BarberShop OwnedBarberShop { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<Report> Reports { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];

    public bool CheckRegisterCompletation()
    {
        if (IsRegisterCompleted)
            return true;

        return this is {
            Email: not null,
            FirstName: not null,
            LastName: not null,
            PhoneNumber: not null,
        };
    }
}
