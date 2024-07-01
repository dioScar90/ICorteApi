using System.Security.Claims;
using ICorteApi.Dtos;
using ICorteApi.Entities;
using ICorteApi.Enums;

namespace ICorteApi.Extensions;

public static class DtoCreator
{
    public static T? CreateDto<T>(this IBaseEntity entity) where T : class, IDtoResponse
    {
        return entity switch
        {
            User user               => MapUserToDto(user) as T,

            Person person           => MapPersonToDto(person) as T,

            BarberShop barberShop   => MapBarberShopToDto(barberShop) as T,

            Address address         => MapAddressToDto(address) as T,

            _ => null
        };
    }

    private static UserRole[] GetRolesAsEnumArray(string[] roles) =>
        roles
            .Select(role => Enum.TryParse<UserRole>(role, out var userRole) ? userRole : (UserRole?)null)
            .Where(role => role.HasValue)
            .Select(role => role.Value)
            .ToArray();
    
    private static UserDtoResponse MapUserToDto(User user) =>
        new(
            user.Id,
            user.Email,
            [],
            // user.Person is null ? default : user.Person.CreateDto<PersonDtoResponse>()
            user.Person?.CreateDto<PersonDtoResponse>()
        );
    
    private static PersonDtoResponse MapPersonToDto(Person person) =>
        new(
            person.Id,
            person.FirstName,
            person.LastName,
            person.LastVisitDate,
            []
            // person.Addresses?.Select(a => a.CreateDto<AddressDtoResponse>()).ToArray()
        );

    private static BarberShopDtoResponse MapBarberShopToDto(BarberShop barberShop) =>
        new(
            barberShop.Name,
            barberShop.Description,
            barberShop.PhoneNumber,
            barberShop.ComercialNumber,
            barberShop.ComercialEmail,
            barberShop.OpeningHours,
            barberShop.ClosingHours,
            barberShop.DaysOpen,
            barberShop.Rating,
            barberShop.Address?.CreateDto<AddressDtoResponse>(),
            barberShop.Barbers?.Select(b => b.CreateDto<PersonDtoResponse>()).ToArray()
        );
        
    private static AddressDtoResponse MapAddressToDto(Address address) =>
        new(
            address.Id,
            address.Street,
            address.Number,
            address.Complement,
            address.Neighborhood,
            address.City,
            address.State,
            address.PostalCode,
            address.Country
        );
}
