using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Presentation.Extensions;

public static class EntityCreator
{
    public static TEntity? CreateEntity<TEntity>(this IDtoRequest<TEntity> dtoRequest)
        where TEntity : class, IBaseTableEntity
    {
        return dtoRequest switch
        {
            UserDtoRequest registerDto
                => MapDtoToUser(registerDto) as TEntity,
                
            UserDtoRegisterRequest registerDto
                => MapDtoToUser(registerDto) as TEntity,
                
            BarberShopDtoRequest barberShopDto
                => MapDtoToBarberShop(barberShopDto) as TEntity,
            
            RecurringScheduleDtoRequest opScheduleDto
                => MapDtoToRecurringSchedule(opScheduleDto) as TEntity,
            
            AddressDtoRequest addressDto
                => MapDtoToAddress(addressDto) as TEntity,
            
            _ => null
        };
    }

    private static User MapDtoToUser(UserDtoRequest userDto) =>
        new()
        {
            UserName = userDto.Email,
            Email = userDto.Email,
            PhoneNumber = userDto.PhoneNumber,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            ImageUrl = userDto.ImageUrl,
        };

    private static User MapDtoToUser(UserDtoRegisterRequest userDto) =>
        new()
        {
            UserName = userDto.Email,
            Email = userDto.Email
        };
        
    private static BarberShop MapDtoToBarberShop(BarberShopDtoRequest barberShopDto) =>
        new()
        {
            Name = barberShopDto.Name,
            Description = barberShopDto.Description ?? default,
            ComercialNumber = barberShopDto.ComercialNumber,
            ComercialEmail = barberShopDto.ComercialEmail,

            Address = barberShopDto.Address?.CreateEntity(),
            RecurringSchedules = barberShopDto.RecurringSchedules?.Select(oh => oh.CreateEntity()).ToList(),
            Barbers = barberShopDto.Barbers?.Select(b => b.CreateEntity()).ToList()
        };

    private static RecurringSchedule MapDtoToRecurringSchedule(RecurringScheduleDtoRequest recurringScheduleDto) =>
        new()
        {
            DayOfWeek = recurringScheduleDto.DayOfWeek,

            // OpenTime = GetTimeSpanValue(recurringScheduleDto.OpenTime),
            // CloseTime = GetTimeSpanValue(recurringScheduleDto.CloseTime),

            OpenTime = recurringScheduleDto.OpenTime,
            CloseTime = recurringScheduleDto.CloseTime,

            BarberShopId = recurringScheduleDto.BarberShopId ?? default,

            IsActive = recurringScheduleDto.IsActive,
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
            Country = addressDto.Country,
            BarberShopId = addressDto.BarberShopId
        };
}
