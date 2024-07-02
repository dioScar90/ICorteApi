using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Presentation.Extensions;

public static class EntityCreator
{
    public static TEntity? CreateEntity<TEntity>(this IDtoRequest? dtoRequest) where TEntity : class, IBaseEntity
    {
        return dtoRequest switch
        {
            UserDtoRegisterRequest registerDto => MapDtoToUser(registerDto) as TEntity,

            PersonDtoRequest personDto => MapDtoToPerson(personDto) as TEntity,

            BarberShopDtoRequest barberShopDto => MapDtoToBarberShop(barberShopDto) as TEntity,

            AddressDtoRequest addressDto => MapDtoToAddress(addressDto) as TEntity,

            _ => null
        };
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
            Barbers = barberShopDto.Barbers?.Select(b => b.CreateEntity<Person>()).ToList()
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
