using ICorteApi.Dtos;
using ICorteApi.Entities;

namespace ICorteApi.Extensions;

public static class EntityCreator
{
    public static TEntity? CreateEntity<TEntity>(this IDtoRequest? dto)
        where TEntity : class, IBaseEntity
    {
        if (dto is UserDtoRegisterRequest registerDto)
            return MapDtoToUser(registerDto) as TEntity;

        if (dto is PersonDtoRequest personDto)
            return MapDtoToPerson(personDto) as TEntity;

        if (dto is BarberShopDtoRequest barberShopDto)
            return MapDtoToBarberShop(barberShopDto) as TEntity;

        if (dto is AddressDtoRequest addressDto)
            return MapDtoToAddress(addressDto) as TEntity;
        
        return default;
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

    private static BarberShop MapDtoToBarberShop(BarberShopDtoRequest barberShopDto) =>
        new()
        {
            Name = barberShopDto.Name,
            Description = barberShopDto.Description,
            PhoneNumber = barberShopDto.PhoneNumber,
            ComercialNumber = barberShopDto.ComercialNumber,
            ComercialEmail = barberShopDto.ComercialEmail,
            OpeningHours = barberShopDto.OpeningHours,
            ClosingHours = barberShopDto.ClosingHours,
            DaysOpen = barberShopDto.DaysOpen,
            Rating = barberShopDto.Rating,
            
            Address = barberShopDto.Address?.CreateEntity<Address>(),
            Barbers = barberShopDto.Barbers?.Select(b => b.CreateEntity<Person>()).ToArray()
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
