using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Presentation.Extensions;

public static class DtoCreator
{
    // public static TDto? CreateDto<TDto, TEntity>(this TEntity entity)
    //     where TEntity : class, IBaseTableEntity
    //     where TDto : class, IDtoResponse<TEntity>
    // {
    //     return entity switch
    //     {
    //         User user
    //             => MapUserToDto(user) as TDto,
            
    //         Person person
    //             => MapPersonToDto(person) as TDto,
            
    //         BarberShop barberShop
    //             => MapBarberShopToDto(barberShop) as TDto,
            
    //         RecurringSchedule opSchedule
    //             => MapRecurringScheduleToDto(opSchedule) as TDto,
            
    //         Address address
    //             => MapAddressToDto(address) as TDto,

    //         _   => default
    //     };
    // }

    public static UserDtoResponse CreateDto(this User user) =>
        new(
            user.Id,
            user.Email,
            [],
            user.Person?.CreateDto()
        );
        
    public static PersonDtoResponse CreateDto(this Person person) =>
        new(
            person.UserId,
            person.FirstName,
            person.LastName,
            person.OwnedBarberShop?.CreateDto()
        );
        
    public static BarberShopDtoResponse CreateDto(this BarberShop barberShop) =>
        new(
            barberShop.Id,
            barberShop.Name,
            barberShop.Description,
            barberShop.ComercialNumber,
            barberShop.ComercialEmail,
            barberShop.Address?.CreateDto(),
            barberShop.RecurringSchedules?.Select(b => b.CreateDto()).ToArray(),
            barberShop.Barbers?.Select(b => b.CreateDto()).ToArray()
        );

    public static RecurringScheduleDtoResponse CreateDto(this RecurringSchedule recurringSchedule) =>
        new(
            recurringSchedule.DayOfWeek,
            recurringSchedule.BarberShopId,
            recurringSchedule.OpenTime,
            recurringSchedule.CloseTime,
            recurringSchedule.IsActive
        );

    public static AddressDtoResponse CreateDto(this Address address) =>
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
