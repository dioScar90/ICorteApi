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
            AddressDtoRequest dto           => MapDtoToAddress(dto) as TEntity,

            AppointmentDtoRequest dto       => MapDtoToAppointment(dto) as TEntity,

            BarberShopDtoRequest dto        => MapDtoToBarberShop(dto) as TEntity,

            MessageDtoRequest dto           => MapDtoToMessage(dto) as TEntity,

            PaymentDtoRequest dto           => MapDtoToPayment(dto) as TEntity,
            
            RecurringScheduleDtoRequest dto => MapDtoToRecurringSchedule(dto) as TEntity,
            
            ReportDtoRequest dto            => MapDtoToReport(dto) as TEntity,
            
            ServiceDtoRequest dto           => MapDtoToService(dto) as TEntity,
            
            SpecialScheduleDtoRequest dto   => MapDtoToSpecialSchedule(dto) as TEntity,
                
            UserDtoRequest dto              => MapDtoToUser(dto) as TEntity,
                
            UserDtoRegisterRequest dto      => MapDtoToUser(dto) as TEntity,

            _ => null
        };
    }

    private static Address MapDtoToAddress(AddressDtoRequest dto) =>
        new()
        {
            Street = dto.Street,
            Number = dto.Number,
            Complement = dto.Complement,
            Neighborhood = dto.Neighborhood,
            City = dto.City,
            State = dto.State,
            PostalCode = dto.PostalCode,
            Country = dto.Country
        };
        
    private static Appointment MapDtoToAppointment(AppointmentDtoRequest dto) =>
        new()
        {
            Date = dto.Date,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime,
            Notes = dto.Notes,
            TotalPrice = dto.TotalPrice,
            Status = dto.Status
        };
        
    private static BarberShop MapDtoToBarberShop(BarberShopDtoRequest dto) =>
        new()
        {
            Name = dto.Name,
            Description = dto.Description ?? default,
            ComercialNumber = dto.ComercialNumber,
            ComercialEmail = dto.ComercialEmail,

            Address = dto.Address?.CreateEntity(),
            RecurringSchedules = dto.RecurringSchedules?.Select(oh => oh.CreateEntity()).ToList(),
            Barbers = dto.Barbers?.Select(b => b.CreateEntity()).ToList()
        };
        
    private static Message MapDtoToMessage(MessageDtoRequest dto) =>
        new()
        {
            Content = dto.Content,
            SentAt = dto.SentAt
        };
        
    private static Payment MapDtoToPayment(PaymentDtoRequest dto) =>
        new()
        {
            PaymentType = dto.PaymentType,
            Amount = dto.Amount
        };

    private static RecurringSchedule MapDtoToRecurringSchedule(RecurringScheduleDtoRequest dto) =>
        new()
        {
            DayOfWeek = dto.DayOfWeek,
            
            OpenTime = dto.OpenTime,
            CloseTime = dto.CloseTime,

            IsActive = dto.IsActive
        };

    private static Report MapDtoToReport(ReportDtoRequest dto) =>
        new()
        {
            Title = dto.Title,
            Content = dto.Content,
            Rating = dto.Rating
        };

    private static Service MapDtoToService(ServiceDtoRequest dto) =>
        new()
        {
            Name = dto.Name,
            Description = dto.Description,
            Price = dto.Price,
        };

    private static SpecialSchedule MapDtoToSpecialSchedule(SpecialScheduleDtoRequest dto) =>
        new()
        {
            Date = dto.Date,
            
            OpenTime = dto.OpenTime,
            CloseTime = dto.CloseTime,
            IsClosed = dto.IsClosed,
        };

    private static User MapDtoToUser(UserDtoRequest dto) =>
        new()
        {
            UserName = dto.Email,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            ImageUrl = dto.ImageUrl,
        };

    private static User MapDtoToUser(UserDtoRegisterRequest dto) =>
        new()
        {
            UserName = dto.Email,
            Email = dto.Email
        };
}
