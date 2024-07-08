using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Presentation.Extensions;

public static class EntityUpdater
{
    public static void UpdateEntityByDto(this IBaseTableEntity entity, IDtoRequest? dtoRequest, DateTime? utcNow = null)
    {
        switch (entity, dtoRequest)
        {
            case (User user, UserDtoRegisterRequest userDto):
                UpdateUserByDto(user, userDto, utcNow);
                break;
            case (Person person, PersonDtoRequest personDto):
                UpdatePersonByDto(person, personDto, utcNow);
                break;
            case (BarberShop barberShop, BarberShopDtoRequest barberShopDto):
                UpdateBarberShopByDto(barberShop, barberShopDto, utcNow);
                break;
            case (OperatingSchedule opSchedule, OperatingScheduleDtoRequest opScheduleDto):
                UpdateOperatingScheduleByDto(opSchedule, opScheduleDto, utcNow);
                break;
            case (Address address, AddressDtoRequest addressDto):
                UpdateAddressByDto(address, addressDto, utcNow);
                break;
            default:
                // Opção para caso nenhum dos padrões acima seja atingido.
                break;
        }
    }

    private static void UpdateUserByDto(User user, UserDtoRegisterRequest dto, DateTime? utcNow = null)
    {
        user.UserName = dto.Email;
        user.Email = dto.Email;
        user.PhoneNumber = dto.PhoneNumber;
        user.Person = dto.PersonDto.CreateEntity<Person>();
    }

    private static void UpdatePersonByDto(Person person, PersonDtoRequest dto, DateTime? utcNow = null)
    {
        utcNow ??= DateTime.UtcNow;

        person.FirstName = dto.FirstName;
        person.LastName = dto.LastName;

        person.UpdatedAt = utcNow;
    }
    
    private static void UpdateBarberShopByDto(BarberShop barberShop, BarberShopDtoRequest dto, DateTime? utcNow = null)
    {
        utcNow ??= DateTime.UtcNow;

        barberShop.Name = dto.Name;
        barberShop.Description = dto.Description ?? null;
        barberShop.ComercialNumber = dto.ComercialNumber;
        barberShop.ComercialEmail = dto.ComercialEmail;

        var itemsToUpdateByOperatingSchedules =
            from os in dto.OperatingSchedules
            join bs in barberShop.OperatingSchedules on os.DayOfWeek equals bs.DayOfWeek
            select new { bs, os };

        foreach (var updatingItems in itemsToUpdateByOperatingSchedules)
            updatingItems.bs.UpdateEntityByDto(updatingItems.os, utcNow);

        if (dto.Address is not null)
            barberShop.Address?.UpdateEntityByDto(dto.Address, utcNow);
        
        barberShop.UpdatedAt = utcNow;
    }

    private static void UpdateOperatingScheduleByDto(OperatingSchedule opSchedule, OperatingScheduleDtoRequest dto, DateTime? utcNow = null)
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
