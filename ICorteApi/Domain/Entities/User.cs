using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public sealed class User : BasePrimaryKeyUserEntity<User, int>
{
    public string? FirstName { get; set; } = "";
    public string? LastName { get; set; } = "";
    public string? ImageUrl { get; set; } = "";

    public bool IsRegisterCompleted { get; set; } = false;
    
    public BarberShop OwnedBarberShop { get; set; }
    public ICollection<Appointment> Appointments { get; set; }
    public ICollection<Report> Reports { get; set; }
    public ICollection<Message> Messages { get; set; }

    public User() {}

    public User(UserDtoRegisterRequest dto)
    {
        UserName = dto.Email;
        Email = dto.Email;
    }

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
    
    private void UpdateByUserDto(UserDtoRequest dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;
        
        PhoneNumber = dto.PhoneNumber;
        FirstName = dto.FirstName;
        LastName = dto.LastName;

        IsRegisterCompleted = CheckRegisterCompletation();

        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<User> requestDto, DateTime? utcNow = null)
    {
        if (requestDto is UserDtoRequest dto)
            UpdateByUserDto(dto, utcNow);
        
        throw new Exception("Dados enviados inválidos");
    }
}
