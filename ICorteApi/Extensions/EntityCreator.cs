using ICorteApi.Dtos;
using ICorteApi.Entities;

namespace ICorteApi.Extensions;

public static class EntityCreator
{
    public static TEntity? CreateEntity<TEntity>(this IDtoRequest dto)
        where TEntity : class, IBaseEntity
    {
        if (dto is UserDtoRegisterRequest registerDto)
            return MapDtoToUser(registerDto) as TEntity;

        if (dto is PersonDtoRequest personDto)
            return MapDtoToPerson(personDto) as TEntity;

        if (dto is AddressDtoRequest addressDto)
            return MapDtoToAddress(addressDto) as TEntity;
        
        return null;
    }
    
    private static User MapDtoToUser(UserDtoRegisterRequest userDto) =>
        new()
        {
            UserName = userDto.Email,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            Person = userDto.Person.CreateEntity<Person>(),
        };
    
    private static Person MapDtoToPerson(PersonDtoRequest personDto) =>
        new()
        {
            FirstName = personDto.FirstName,
            LastName = personDto.LastName,
            // Addresses = personDto.Addresses?.Select(a => a.CreateEntity<Address>()).ToList(),
        };
        
    private static Address MapDtoToAddress(AddressDtoRequest addressDto) =>
        new()
        {
            Street = addressDto.Street,
            Number = addressDto.Number,
            Complement = addressDto.Complement,
            Neighborhood = addressDto.Neighborhood,
            City = addressDto.City,
            State = addressDto.State,
            PostalCode = addressDto.PostalCode,
            Country = addressDto.Country
        };
}
