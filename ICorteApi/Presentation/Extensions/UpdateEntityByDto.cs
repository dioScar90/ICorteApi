// using ICorteApi.Application.Dtos;
// using ICorteApi.Application.Interfaces;
// using ICorteApi.Domain.Entities;
// using ICorteApi.Domain.Interfaces;

// namespace ICorteApi.Presentation.Extensions;

// public static class EntityUpdater
// {
//     public static void UpdateEntityByDto<TEntity, TDto>(
//         this TEntity entity, TDto? dtoRequest, DateTime? utcNow = null)
//         where TEntity : class, IBaseTableEntity
//         where TDto : IDtoRequest<TEntity>
//     {
//         switch (entity, dtoRequest)
//         {
//             case (Address address, AddressDtoRequest addressDto):
//                 UpdateAddressByDto(address, addressDto, utcNow);
//                 break;
//             case (Appointment appointment, AppointmentDtoRequest appointmentDto):
//                 UpdateAppointmentByDto(appointment, appointmentDto, utcNow);
//                 break;
//             case (BarberShop barberShop, BarberShopDtoRequest barberShopDto):
//                 UpdateBarberShopByDto(barberShop, barberShopDto, utcNow);
//                 break;
//             case (Message message, MessageDtoRequest messageDto):
//                 UpdateMessageByDto(message, messageDto, utcNow);
//                 break;
//             case (Payment payment, PaymentDtoRequest paymentDto):
//                 UpdatePaymentByDto(payment, paymentDto, utcNow);
//                 break;
//             case (RecurringSchedule schedule, RecurringScheduleDtoRequest scheduleDto):
//                 UpdateRecurringScheduleByDto(schedule, scheduleDto, utcNow);
//                 break;
//             case (Report report, ReportDtoRequest reportDto):
//                 UpdateReportByDto(report, reportDto, utcNow);
//                 break;
//             case (Service service, ServiceDtoRequest serviceDto):
//                 UpdateServiceByDto(service, serviceDto, utcNow);
//                 break;
//             case (SpecialSchedule schedule, SpecialScheduleDtoRequest scheduleDto):
//                 UpdateSpecialScheduleByDto(schedule, scheduleDto, utcNow);
//                 break;
//             case (User user, UserDtoRequest userDto):
//                 UpdateUserByDto(user, userDto, utcNow);
//                 break;
//             default:
//                 throw new ArgumentException("", nameof(dtoRequest));
//         }
//     }

//     private static void UpdateAddressByDto(Address address, AddressDtoRequest dto, DateTime? utcNow = null)
//     {
//         utcNow ??= DateTime.UtcNow;

//         address.Street = dto.Street;
//         address.Number = dto.Number;
//         address.Complement = dto.Complement;
//         address.Neighborhood = dto.Neighborhood;
//         address.City = dto.City;
//         address.State = dto.State;
//         address.PostalCode = dto.PostalCode;
//         address.Country = dto.Country;

//         address.UpdatedAt = utcNow;
//     }
    
//     private static void UpdateAppointmentByDto(Appointment appointment, AppointmentDtoRequest dto, DateTime? utcNow = null)
//     {
//         utcNow ??= DateTime.UtcNow;

//         appointment.Date = dto.Date;
//         appointment.StartTime = dto.StartTime;
//         appointment.EndTime = dto.EndTime;
//         appointment.Notes = dto.Notes;
//         appointment.TotalPrice = dto.TotalPrice;
//         appointment.Status = dto.Status;
        
//         appointment.UpdatedAt = utcNow;
//     }
    
//     private static void UpdateMessageByDto(Message message, MessageDtoRequest dto, DateTime? utcNow = null)
//     {
//         utcNow ??= DateTime.UtcNow;

//         message.Content = dto.Content;
        
//         message.UpdatedAt = utcNow;
//     }
    
//     private static void UpdatePaymentByDto(Payment payment, PaymentDtoRequest dto, DateTime? utcNow = null)
//     {
//         utcNow ??= DateTime.UtcNow;

//         payment.PaymentType = dto.PaymentType;
//         payment.Amount = dto.Amount;
        
//         payment.UpdatedAt = utcNow;
//     }
    
//     private static void UpdateRecurringScheduleByDto(RecurringSchedule schedule, RecurringScheduleDtoRequest dto, DateTime? utcNow = null)
//     {
//         utcNow ??= DateTime.UtcNow;

//         schedule.OpenTime = dto.OpenTime;
//         schedule.CloseTime = dto.CloseTime;

//         schedule.UpdatedAt = utcNow;
//     }
    
//     private static void UpdateReportByDto(Report report, ReportDtoRequest dto, DateTime? utcNow = null)
//     {
//         utcNow ??= DateTime.UtcNow;

//         report.Title = dto.Title;
//         report.Content = dto.Content;
//         report.Rating = dto.Rating;
        
//         report.UpdatedAt = utcNow;
//     }
    
//     private static void UpdateServiceByDto(Service service, ServiceDtoRequest dto, DateTime? utcNow = null)
//     {
//         utcNow ??= DateTime.UtcNow;

//         service.Name = dto.Name;
//         service.Description = dto.Description;
//         service.Price = dto.Price;
        
//         service.UpdatedAt = utcNow;
//     }

//     private static void UpdateSpecialScheduleByDto(SpecialSchedule schedule, SpecialScheduleDtoRequest dto, DateTime? utcNow = null)
//     {
//         utcNow ??= DateTime.UtcNow;

//         schedule.OpenTime = dto.OpenTime;
//         schedule.CloseTime = dto.CloseTime;
//         schedule.IsClosed = dto.IsClosed;

//         schedule.UpdatedAt = utcNow;
//     }
    
//     private static void UpdateUserByDto(User user, UserDtoRequest dto, DateTime? utcNow = null)
//     {
//         utcNow ??= DateTime.UtcNow;
        
//         user.PhoneNumber = dto.PhoneNumber;
//         user.FirstName = dto.FirstName;
//         user.LastName = dto.LastName;

//         user.UpdatedAt = utcNow;
//     }
// }
