using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public sealed class Person : BasePrimaryKeyEntity<Person, int>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Gender Gender { get; private set; }
    public string? ImageUrl { get; private set; }

    public User User { get; init; }

    private Person() { }

    public Person(PersonDtoRegisterRequest dto, int userId)
    {
        Id = userId;
        FirstName = dto.FirstName;
        LastName = dto.LastName;
        Gender = dto.Gender;
    }

    private void UpdateByUserDto(PersonDtoRequest dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        FirstName = dto.FirstName;
        LastName = dto.LastName;
        Gender = dto.Gender;

        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<Person> requestDto, DateTime? utcNow = null)
    {
        if (requestDto is PersonDtoRequest dto)
            UpdateByUserDto(dto, utcNow);

        throw new Exception("Dados enviados inválidos");
    }
}

public enum Gender
{
    Male,
    Female,
    Other
}