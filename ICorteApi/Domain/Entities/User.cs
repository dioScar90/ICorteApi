using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Domain.Entities;

public sealed class User : IdentityUser<int>, IPrimaryKeyEntity<User, int>
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; protected set; }
    public bool IsDeleted { get; protected set; } = false;

    public Person Person { get; set; }
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
    
    public void UpdatedUserNow() => UpdatedAt = DateTime.UtcNow;
    
    public void UpdateEntityByDto(IDtoRequest<User> requestDto, DateTime? utcNow = null)
    {
        throw new NotImplementedException();
    }

    public void DeleteEntity()
    {
        if (IsDeleted)
            throw new Exception("Já está excluído");
        
        UpdatedAt = DateTime.UtcNow;
        IsDeleted = true;
    }
}
