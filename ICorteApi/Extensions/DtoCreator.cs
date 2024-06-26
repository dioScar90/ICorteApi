using System.Security.Claims;
using ICorteApi.Dtos;
using ICorteApi.Entities;
using ICorteApi.Enums;

namespace ICorteApi.Extensions;

public static class DtoCreator
{
    public static TDtoResponse? CreateDto<TDtoResponse>(this IBaseEntity entity)
        where TDtoResponse : class, IDtoResponse
    {
        if (entity is User user)
            return MapUserToDto(user) as TDtoResponse;

        if (entity is Person person)
            return MapPersonToDto(person) as TDtoResponse;

        if (entity is Address address)
            return MapAddressToDto(address) as TDtoResponse;
        
        return null;
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
            user.Person.FirstName,
            user.Person.LastName,
            user.Email,
            [],
            user.Person.CreateDto<PersonDtoResponse>()
        );
    
    private static PersonDtoResponse MapPersonToDto(Person person) =>
        new(
            person.Id,
            person.FirstName,
            person.LastName,
            []
            // person.Addresses?.Select(a => a.CreateDto<AddressDtoResponse>()).ToArray()
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
