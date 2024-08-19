using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Domain.Entities;

public sealed class User : BaseUserEntity
{
    public Profile Profile { get; set; }
    public BarberShop OwnedBarberShop { get; set; }
    public ICollection<Appointment> Appointments { get; set; } = [];
    public ICollection<Report> Reports { get; set; } = [];
    public ICollection<Message> Messages { get; set; } = [];

    private readonly HashSet<UserRole> _roles = [];

    public User() { }

    public User(UserDtoRegisterRequest dto)
    {
        UserName = dto.Email;
        Email = dto.Email;
    }

    public void SetRoles(UserRole[] roles)
    {
        foreach (var role in roles)
            _roles.Add(role);
    }

    private string[] GetRolesAsStringArray() => _roles.Select(Enum.GetName).ToArray()!;

    public override UserDtoResponse CreateDto() =>
        new(
            Id,
            Email,
            PhoneNumber,
            GetRolesAsStringArray(),
            Profile?.CreateDto(),
            OwnedBarberShop?.CreateDto()
        );
}
