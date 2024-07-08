using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Presentation.Extensions;

public static class DtoCreator
{
    public static TDto? CreateDto<TDto>(this IBaseTableEntity entity) where TDto : class, IDtoResponse
    {
        return entity switch
        {
            User user                       => MapUserToDto(user) as TDto,
            Person person                   => MapPersonToDto(person) as TDto,
            BarberShop barberShop           => MapBarberShopToDto(barberShop) as TDto,
            OperatingSchedule opSchedule    => MapOperatingScheduleToDto(opSchedule) as TDto,
            Address address                 => MapAddressToDto(address) as TDto,
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
            person.UserId,
            person.FirstName,
            person.LastName,
            person.LastVisitDate,
            [],
            person.OwnedBarberShop?.CreateDto<BarberShopDtoResponse>()
        );

    private static BarberShopDtoResponse MapBarberShopToDto(BarberShop barberShop) =>
        new(
            barberShop.Id,
            barberShop.Name,
            barberShop.Description,
            barberShop.ComercialNumber,
            barberShop.ComercialEmail,
            barberShop.Rating,
            barberShop.Address?.CreateDto<AddressDtoResponse>(),
            barberShop.OperatingSchedules?.Select(b => b.CreateDto<OperatingScheduleDtoResponse>()).ToArray(),
            barberShop.Barbers?.Select(b => b.CreateDto<PersonDtoResponse>()).ToArray()
        );

    private static OperatingScheduleDtoResponse MapOperatingScheduleToDto(OperatingSchedule operatingSchedule) =>
        new(
            operatingSchedule.DayOfWeek,
            operatingSchedule.BarberShopId,
            operatingSchedule.OpenTime,
            operatingSchedule.CloseTime,
            operatingSchedule.IsActive
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
