// using ICorteApi.Application.Dtos;
// using ICorteApi.Application.Interfaces;
// using ICorteApi.Domain.Entities;
// using ICorteApi.Domain.Interfaces;

// namespace ICorteApi.Presentation.Extensions;

// public static class EntityCreator
// {
//     public static TEntity? CreateEntity<TEntity>(this IDtoRequest<TEntity> dtoRequest)
//         where TEntity : class, IBaseTableEntity
//     {
//         return dtoRequest switch
//         {
//             AddressDtoRequest dto           => MapDtoToAddress(dto) as TEntity,

//             AppointmentDtoRequest dto       => MapDtoToAppointment(dto) as TEntity,

//             BarberShopDtoRequest dto        => MapDtoToBarberShop(dto) as TEntity,

//             MessageDtoRequest dto           => MapDtoToMessage(dto) as TEntity,

//             PaymentDtoRequest dto           => MapDtoToPayment(dto) as TEntity,
            
//             RecurringScheduleDtoRequest dto => MapDtoToRecurringSchedule(dto) as TEntity,
            
//             ReportDtoRequest dto            => MapDtoToReport(dto) as TEntity,
            
//             ServiceDtoRequest dto           => MapDtoToService(dto) as TEntity,
            
//             SpecialScheduleDtoRequest dto   => MapDtoToSpecialSchedule(dto) as TEntity,
            
//             UserDtoRegisterRequest dto      => MapDtoToUser(dto) as TEntity,

//             _ => null
//         };
//     }

//     private static Address MapDtoToAddress(AddressDtoRequest dto) => new(dto);
        
//     private static Appointment MapDtoToAppointment(AppointmentDtoRequest dto) => new(dto);
        
//     private static BarberShop MapDtoToBarberShop(BarberShopDtoRequest dto) =>
//         new(dto);
        
//     private static Message MapDtoToMessage(MessageDtoRequest dto) =>
//         new(dto);
        
//     private static Payment MapDtoToPayment(PaymentDtoRequest dto) =>
//         new(dto);

//     private static RecurringSchedule MapDtoToRecurringSchedule(RecurringScheduleDtoRequest dto) =>
//         new(dto);

//     private static Report MapDtoToReport(ReportDtoRequest dto) =>
//         new(dto);

//     private static Service MapDtoToService(ServiceDtoRequest dto) =>
//         new(dto);

//     private static SpecialSchedule MapDtoToSpecialSchedule(SpecialScheduleDtoRequest dto) =>
//         new(dto);
        
//     private static User MapDtoToUser(UserDtoRegisterRequest dto) =>
//         new(dto);
// }
