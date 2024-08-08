using FluentValidation;
using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Services;
using ICorteApi.Application.Validators;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Infraestructure.Repositories;

namespace ICorteApi.Presentation.Extensions;

public static class ServiceCollectionExtensions
{    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // var interfaceType = typeof(IBaseRepository<>);
        // var types = assembly.GetTypes()
        //     .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
        //         .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType))
        //     .ToList();

        // foreach (var implementationType in types)
        // {
        //     var specificInterface = implementationType.GetInterfaces()
        //         .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);

        //     if (specificInterface != null)
        //     {
        //         services.AddScoped(specificInterface, implementationType);
        //     }
        // }

        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IPaymentService, PaymentService>();
        services.AddScoped<IServiceAppointmentService, ServiceAppointmentService>();
        services.AddScoped<IBarberShopService, BarberShopService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IRecurringScheduleService, RecurringScheduleService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<ISpecialScheduleService, SpecialScheduleService>();
        services.AddScoped<IUserService, UserService>();
        
        return services;
    }
    
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        // var interfaceType = typeof(IBaseService<>);
        // var types = assembly.GetTypes()
        //     .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
        //         .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType))
        //     .ToList();

        // foreach (var implementationType in types)
        // {
        //     var specificInterface = implementationType.GetInterfaces()
        //         .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);

        //     if (specificInterface != null)
        //     {
        //         services.AddScoped(specificInterface, implementationType);
        //     }
        // }

        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IServiceAppointmentRepository, ServiceAppointmentRepository>();
        services.AddScoped<IBarberShopRepository, BarberShopRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IRecurringScheduleRepository, RecurringScheduleRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<ISpecialScheduleRepository, SpecialScheduleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
    
    public static IServiceCollection AddErrors(this IServiceCollection services)
    {
        // var interfaceType = typeof(IBaseErrors<>);
        // var types = assembly.GetTypes()
        //     .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
        //         .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType))
        //     .ToList();

        // foreach (var implementationType in types)
        // {
        //     var specificInterface = implementationType.GetInterfaces()
        //         .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == interfaceType);

        //     if (specificInterface != null)
        //     {
        //         services.AddScoped(specificInterface, implementationType);
        //     }
        // }

        services.AddScoped<IAddressErrors, AddressErrors>();
        services.AddScoped<IAppointmentErrors, AppointmentErrors>();
        services.AddScoped<IPaymentErrors, PaymentErrors>();
        services.AddScoped<IBarberShopErrors, BarberShopErrors>();
        services.AddScoped<IMessageErrors, MessageErrors>();
        services.AddScoped<IRecurringScheduleErrors, RecurringScheduleErrors>();
        services.AddScoped<IReportErrors, ReportErrors>();
        services.AddScoped<IServiceErrors, ServiceErrors>();
        services.AddScoped<ISpecialScheduleErrors, SpecialScheduleErrors>();
        services.AddScoped<IUserErrors, UserErrors>();

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AddressDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<AppointmentDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<PaymentDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<BarberShopDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<MessageDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<RecurringScheduleDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ReportDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ServiceDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SpecialScheduleDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UserDtoLoginRequestValidator>();

        return services;
    }
}
