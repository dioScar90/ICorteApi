using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Presentation.Extensions;

public static class EntityUpdater
{
    public static void UpdateEntityByDto<TEntity>(
        this TEntity entity, IDtoRequest<TEntity>? dtoRequest, DateTime? utcNow = null)
        where TEntity : class, IBaseTableEntity
    {
        switch (entity, dtoRequest)
        {
            case (User user, UserDtoRequest userDto):
                UpdateUserByDto(user, userDto, utcNow);
                break;
            case (BarberShop barberShop, BarberShopDtoRequest barberShopDto):
                UpdateBarberShopByDto(barberShop, barberShopDto, utcNow);
                break;
            case (RecurringSchedule opSchedule, RecurringScheduleDtoRequest opScheduleDto):
                UpdateRecurringScheduleByDto(opSchedule, opScheduleDto, utcNow);
                break;
            case (Address address, AddressDtoRequest addressDto):
                UpdateAddressByDto(address, addressDto, utcNow);
                break;
            default:
                throw new ArgumentException("", nameof(dtoRequest));
        }
    }
    
    private static void UpdateUserByDto(User user, UserDtoRequest dto, DateTime? utcNow = null)
    {
        utcNow ??= DateTime.UtcNow;
        
        user.UserName = dto.Email;
        user.Email = dto.Email;
        user.PhoneNumber = dto.PhoneNumber;
        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;

        user.UpdatedAt = utcNow;
    }
    
    private static void UpdateBarberShopByDto(BarberShop barberShop, BarberShopDtoRequest dto, DateTime? utcNow = null)
    {
        utcNow ??= DateTime.UtcNow;

        barberShop.Name = dto.Name;
        barberShop.Description = dto.Description ?? null;
        barberShop.ComercialNumber = dto.ComercialNumber;
        barberShop.ComercialEmail = dto.ComercialEmail;

        var itemsToUpdateByRecurringSchedules =
            from os in dto.RecurringSchedules
            join bs in barberShop.RecurringSchedules on os.DayOfWeek equals bs.DayOfWeek
            select new { bs, os };

        foreach (var updatingItems in itemsToUpdateByRecurringSchedules)
            updatingItems.bs.UpdateEntityByDto(updatingItems.os, utcNow);

        if (dto.Address is not null)
            barberShop.Address?.UpdateEntityByDto(dto.Address, utcNow);

        barberShop.UpdatedAt = utcNow;
    }

    private static void UpdateRecurringScheduleByDto(RecurringSchedule opSchedule, RecurringScheduleDtoRequest dto, DateTime? utcNow = null)
    {
        utcNow ??= DateTime.UtcNow;

        opSchedule.OpenTime = dto.OpenTime;
        opSchedule.CloseTime = dto.CloseTime;

        opSchedule.UpdatedAt = utcNow;
    }

    private static void UpdateAddressByDto(Address address, AddressDtoRequest dto, DateTime? utcNow = null)
    {
        utcNow ??= DateTime.UtcNow;

        address.Street = dto.Street;
        address.Number = dto.Number;
        address.Complement = dto.Complement;
        address.Neighborhood = dto.Neighborhood;
        address.City = dto.City;
        address.State = dto.State;
        address.PostalCode = dto.PostalCode;
        address.Country = dto.Country;

        address.UpdatedAt = utcNow;
    }
}
