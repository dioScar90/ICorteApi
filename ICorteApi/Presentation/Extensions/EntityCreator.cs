using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Presentation.Extensions;

public static class EntityCreator
{
    public static TEntity? CreateEntity<TEntity>(this IDtoRequest? dtoRequest) where TEntity : class, IBaseTableEntity
    {
        return dtoRequest switch
        {
            UserDtoRegisterRequest registerDto => MapDtoToUser(registerDto) as TEntity,
            PersonDtoRequest personDto => MapDtoToPerson(personDto) as TEntity,
            BarberShopDtoRequest barberShopDto => MapDtoToBarberShop(barberShopDto) as TEntity,
            RecurringScheduleDtoRequest opScheduleDto => MapDtoToRecurringSchedule(opScheduleDto) as TEntity,
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
            Person = userDto.PersonDto.CreateEntity<Person>(),
        };

    private static Person MapDtoToPerson(PersonDtoRequest personDto) =>
        new()
        {
            FirstName = personDto.FirstName,
            LastName = personDto.LastName,
            // Addresses = personDto.Addresses?.Select(a => a.CreateEntity<Address>()).ToList(),
        };

    private static TimeSpan GetTimeSpanValue(string value) =>
        TimeSpan.TryParse(value, out TimeSpan timeSpan) ? timeSpan : default;

    // private static Dictionary<DayOfWeek, (TimeSpan, TimeSpan)> GetRecurringScheduleDictionary(
    //     Dictionary<DayOfWeek, (string, string)> recurringSchedule)
    // {
    //     var newDict = new Dictionary<DayOfWeek, (TimeSpan, TimeSpan)>();

    //     foreach (var item in recurringSchedule)
    //     {
    //         newDict.Add(item.Key, (GetTimeSpanValue(item.Value.Item1), GetTimeSpanValue(item.Value.Item1)));
    //     }

    //     return newDict;
    // }

    private static Dictionary<DayOfWeek, (TimeSpan, TimeSpan)> GetRecurringScheduleDictionary(
        Dictionary<DayOfWeek, (string, string)> recurringSchedule) =>
        recurringSchedule.ToDictionary(
            item => item.Key,
            item => (GetTimeSpanValue(item.Value.Item1), GetTimeSpanValue(item.Value.Item2))
        );

    private static BarberShop MapDtoToBarberShop(BarberShopDtoRequest barberShopDto) =>
        new()
        {
            Name = barberShopDto.Name,
            Description = barberShopDto.Description ?? default,
            ComercialNumber = barberShopDto.ComercialNumber,
            ComercialEmail = barberShopDto.ComercialEmail,

            Address = barberShopDto.Address?.CreateEntity<Address>(),
            RecurringSchedules = barberShopDto.RecurringSchedules?.Select(oh => oh.CreateEntity<RecurringSchedule>()).ToList(),
            Barbers = barberShopDto.Barbers?.Select(b => b.CreateEntity<Person>()).ToList()
        };

    private static RecurringSchedule MapDtoToRecurringSchedule(RecurringScheduleDtoRequest recurringScheduleDto) =>
        new()
        {
            DayOfWeek = recurringScheduleDto.DayOfWeek,

            // OpenTime = GetTimeSpanValue(recurringScheduleDto.OpenTime),
            // CloseTime = GetTimeSpanValue(recurringScheduleDto.CloseTime),

            OpenTime = recurringScheduleDto.OpenTime,
            CloseTime = recurringScheduleDto.CloseTime,

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
            Country = addressDto.Country
        };
}
