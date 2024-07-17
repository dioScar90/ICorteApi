// using ICorteApi.Application.Dtos;
// using ICorteApi.Application.Interfaces;
// using ICorteApi.Domain.Entities;
// using ICorteApi.Domain.Enums;
// using ICorteApi.Domain.Errors;
// using ICorteApi.Domain.Interfaces;

// namespace ICorteApi.Presentation.Extensions;

// public static class DtoValidator
// {
//     public static Error[] ValidateDto<TEntity>(this IDtoRequest? dtoRequest) where TEntity : class, IBaseTableEntity
//     {
//         return dtoRequest switch
//         {
//             UserDtoRegisterRequest registerDto          => MapDtoToUser(registerDto) as TEntity!,
//             PersonDtoRequest personDto                  => MapDtoToPerson(personDto) as TEntity!,
//             BarberShopDtoRequest barberShopDto          => MapDtoToBarberShop(barberShopDto) as TEntity!,
//             OperatingScheduleDtoRequest opScheduleDto   => MapDtoToOperatingSchedule(opScheduleDto) as TEntity!,
//             AddressDtoRequest addressDto                => MapDtoToAddress(addressDto) as TEntity!,
//             _ => null
//         };
//     }

//     private static User MapDtoToUser(UserDtoRegisterRequest userDto) =>
//         new()
//         {
//             UserName = userDto.Email,
//             Email = userDto.Email,
//             PhoneNumber = userDto.PhoneNumber,
//             Person = userDto.PersonDto.CreateEntity<Person>(),
//         };

//     private static Person MapDtoToPerson(PersonDtoRequest personDto) =>
//         new()
//         {
//             FirstName = personDto.FirstName,
//             LastName = personDto.LastName,
//             // Addresses = personDto.Addresses?.Select(a => a.CreateEntity<Address>()).ToList(),
//         };

//     private static TimeSpan GetTimeSpanValue(string value) =>
//         TimeSpan.TryParse(value, out TimeSpan timeSpan) ? timeSpan : default;

//     // private static Dictionary<DayOfWeek, (TimeSpan, TimeSpan)> GetOperatingScheduleDictionary(
//     //     Dictionary<DayOfWeek, (string, string)> operatingSchedule)
//     // {
//     //     var newDict = new Dictionary<DayOfWeek, (TimeSpan, TimeSpan)>();

//     //     foreach (var item in operatingSchedule)
//     //     {
//     //         newDict.Add(item.Key, (GetTimeSpanValue(item.Value.Item1), GetTimeSpanValue(item.Value.Item1)));
//     //     }

//     //     return newDict;
//     // }

//     private static Dictionary<DayOfWeek, (TimeSpan, TimeSpan)> GetOperatingScheduleDictionary(
//         Dictionary<DayOfWeek, (string, string)> operatingSchedule) =>
//         operatingSchedule.ToDictionary(
//             item => item.Key,
//             item => (GetTimeSpanValue(item.Value.Item1), GetTimeSpanValue(item.Value.Item2))
//         );

//     private static BarberShop MapDtoToBarberShop(BarberShopDtoRequest barberShopDto) =>
//         new()
//         {
//             Name = barberShopDto.Name,
//             Description = barberShopDto.Description ?? default,
//             ComercialNumber = barberShopDto.ComercialNumber,
//             ComercialEmail = barberShopDto.ComercialEmail,

//             Address = barberShopDto.Address?.CreateEntity<Address>(),
//             OperatingSchedules = barberShopDto.OperatingSchedules?.Select(oh => oh.CreateEntity<OperatingSchedule>()).ToList(),
//             Barbers = barberShopDto.Barbers?.Select(b => b.CreateEntity<Person>()).ToList()
//         };

//     private static OperatingSchedule MapDtoToOperatingSchedule(OperatingScheduleDtoRequest operatingScheduleDto) =>
//         new()
//         {
//             DayOfWeek = operatingScheduleDto.DayOfWeek,

//             // OpenTime = GetTimeSpanValue(operatingScheduleDto.OpenTime),
//             // CloseTime = GetTimeSpanValue(operatingScheduleDto.CloseTime),

//             OpenTime = operatingScheduleDto.OpenTime,
//             CloseTime = operatingScheduleDto.CloseTime,

//             IsActive = operatingScheduleDto.IsActive,
//         };

//     private static Address MapDtoToAddress(AddressDtoRequest addressDto) =>
//         new()
//         {
//             Street = addressDto.Street,
//             Number = addressDto.Number,
//             Complement = addressDto.Complement,
//             Neighborhood = addressDto.Neighborhood,
//             City = addressDto.City,
//             State = addressDto.State,
//             PostalCode = addressDto.PostalCode,
//             Country = addressDto.Country
//         };
// }
