using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public sealed class Profile : BasePrimaryKeyEntity<Profile, int>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Gender Gender { get; private set; }
    public string? ImageUrl { get; private set; }

    public User User { get; init; }

    private Profile() { }

    public Profile(ProfileDtoRegisterRequest dto, int userId)
    {
        Id = userId;
        FirstName = dto.FirstName;
        LastName = dto.LastName;
        Gender = dto.Gender;
    }

    private void UpdateByUserDto(ProfileDtoRequest dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        FirstName = dto.FirstName;
        LastName = dto.LastName;
        Gender = dto.Gender;

        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<Profile> requestDto, DateTime? utcNow = null)
    {
        if (requestDto is ProfileDtoRequest dto)
            UpdateByUserDto(dto, utcNow);

        throw new Exception("Dados enviados inválidos");
    }

    public override ProfileDtoResponse CreateDto() =>
        new(
            Id,
            FirstName,
            LastName,
            Gender,
            ImageUrl
        );
}

public enum Gender
{
    Female,
    Male,
    Other
}