using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Presentation.Extensions;

public static class DtoCreator
{
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
        
    public static SpecialScheduleDtoResponse CreateDto(this SpecialSchedule specialSchedule) =>
        new(
            specialSchedule.Date,
            specialSchedule.BarberShopId,
            specialSchedule.Notes,
            specialSchedule.OpenTime,
            specialSchedule.CloseTime,
            specialSchedule.IsClosed
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
