using ICorteApi.Dtos;
using ICorteApi.Entities;

namespace ICorteApi.Extensions;

public static class Mapper
{
    public static TDtoResponse? CreateDto<TDtoResponse>(this IBaseEntity entity)
        where TDtoResponse : class, IDtoResponse
    {
        if (entity is User user)
            return MapUserToDto(user) as TDtoResponse;

        if (entity is Address address)
            return MapAddressToDto(address) as TDtoResponse;
        
        return null;
    }

    public static TEntity? CreateEntity<TEntity>(this IDtoRequest dto)
        where TEntity : class, IBaseEntity
    {
        if (dto is UserDtoRequest userDto)
            return MapDtoToUser(userDto) as TEntity;

        if (dto is AddressDtoRequest addressDto)
            return MapDtoToAddress(addressDto) as TEntity;
        
        return null;
    }
    
    private static UserDtoResponse MapUserToDto(User user) =>
        new(
            user.Id,
            user.FirstName,
            user.LastName,
            user.Email,
            user.PhoneNumber,
            user.Role,
            user.Addresses?.Select(a => a.CreateDto<AddressDtoResponse>()) ?? []
        );
    
    private static User MapDtoToUser(UserDtoRequest userDto) =>
        new()
        {
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            Role = userDto.Role,
            Addresses = userDto.Addresses?.Select(a => a.CreateEntity<Address>()) ?? []
        };

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
