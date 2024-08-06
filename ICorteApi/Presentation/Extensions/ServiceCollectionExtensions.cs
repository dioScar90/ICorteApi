using System.Reflection;
using FluentValidation;
using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Services;
using ICorteApi.Application.Validators;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Infraestructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ICorteApi.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    // public static void AddApplicationServices(this IServiceCollection services, Assembly assembly)
    // {
    //     // Registrar Services
    //     var serviceTypes = assembly.GetTypes()
    //         .Where(t => t.IsClass && t.IsSealed && !t.IsAbstract && t.GetInterfaces()
    //             .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseService<>)))
    //         .ToList();

    //     foreach (var serviceType in serviceTypes)
    //     {
    //         var serviceInterface = serviceType.GetInterfaces()
    //             .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseService<>));

    //         services.AddScoped(serviceInterface, serviceType);
    //     }

    //     // Registrar Repositories
    //     var repositoryTypes = assembly.GetTypes()
    //         .Where(t => t.IsClass && t.IsSealed && !t.IsAbstract && t.GetInterfaces()
    //             .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseRepository<>)))
    //         .ToList();

    //     foreach (var repositoryType in repositoryTypes)
    //     {
    //         var repositoryInterface = repositoryType.GetInterfaces()
    //             .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseRepository<>));

    //         services.AddScoped(repositoryInterface, repositoryType);
    //     }

    //     // Registrar Errors
    //     var errorTypes = assembly.GetTypes()
    //         .Where(t => t.IsClass && t.IsSealed && !t.IsAbstract && t.GetInterfaces()
    //             .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseErrors<>)))
    //         .ToList();

    //     foreach (var errorType in errorTypes)
    //     {
    //         var errorInterface = errorType.GetInterfaces()
    //             .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IBaseErrors<>));

    //         services.AddScoped(errorInterface, errorType);
    //     }
    // }







    // public static void AddApplicationServices(this IServiceCollection services, Assembly assembly)
    // {
    //     IterateThroughTypeAndAddScopedByReflection(services, assembly, typeof(IBaseRepository<>));
    //     IterateThroughTypeAndAddScopedByReflection(services, assembly, typeof(IBaseService<>));
    //     IterateThroughTypeAndAddScopedByReflection(services, assembly, typeof(IBaseErrors<>));
    // }

    // private static void IterateThroughTypeAndAddScopedByReflection(
    //     IServiceCollection services, Assembly assembly, Type genericType)
    // {
    //     var types = assembly.GetTypes()
    //         .Where(t => t.IsClass
    //                     && !t.IsAbstract
    //                     && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType))
    //         .ToList();

    //     foreach (var type in types)
    //     {
    //         var interfaceType = type.GetInterfaces()
    //             .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType);

    //         if (interfaceType is not null)
    //         {
    //             services.AddScoped(interfaceType, type);
    //         }
    //     }
    // }





    // public static void AddApplicationServices(this IServiceCollection services, Assembly assembly)
    // {
    //     IterateThroughTypeAndAddScopedByReflection(services, assembly, typeof(IBasePrimaryKeyRepository<,>)); // Note the change to a 2-type generic
    //     IterateThroughTypeAndAddScopedByReflection(services, assembly, typeof(IBaseCompositeKeyRepository<,,>)); // Note the change to a 2-type generic
    //     IterateThroughTypeAndAddScopedByReflection(services, assembly, typeof(IBasePrimaryKeyService<,>));
    //     IterateThroughTypeAndAddScopedByReflection(services, assembly, typeof(IBaseCompositeKeyService<,,>));
    //     IterateThroughTypeAndAddScopedByReflection(services, assembly, typeof(IBaseErrors<>));
    // }

    // private static void IterateThroughTypeAndAddScopedByReflection(
    //     IServiceCollection services, Assembly assembly, Type genericType)
    // {
    //     var types = assembly.GetTypes()
    //         .Where(t => t.IsClass
    //                     && !t.IsAbstract
    //                     && t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType))
    //         .ToList();

    //     foreach (var type in types)
    //     {
    //         var interfaceType = type.GetInterfaces()
    //             .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType);

    //         if (interfaceType is not null)
    //         {
    //             services.AddScoped(interfaceType, type);
    //         }
    //     }
    // }











    // public static void AddApplicationServices(this IServiceCollection services, Assembly assembly)
    // {
    //     RegisterRepositories(services, assembly);
    //     RegisterServices(services, assembly);
    //     RegisterErrors(services, assembly);
    // }

    // public static IServiceCollection AddRepositories(this IServiceCollection services, Assembly assembly)
    public static IServiceCollection AddRepositories(this IServiceCollection services)
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
        services.AddScoped<IServiceAppointmentService, ServiceAppointmentService>();
        services.AddScoped<IBarberShopService, BarberShopService>();
        services.AddScoped<IConversationParticipantService, ConversationParticipantService>();
        services.AddScoped<IConversationService, ConversationService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IPersonService, PersonService>();
        services.AddScoped<IRecurringScheduleService, RecurringScheduleService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<ISpecialScheduleService, SpecialScheduleService>();
        services.AddScoped<IUserService, UserService>();
        
        return services;
    }

    // public static IServiceCollection AddServices(this IServiceCollection services, Assembly assembly)
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
        services.AddScoped<IServiceAppointmentRepository, ServiceAppointmentRepository>();
        services.AddScoped<IBarberShopRepository, BarberShopRepository>();
        services.AddScoped<IConversationParticipantRepository, ConversationParticipantRepository>();
        services.AddScoped<IConversationRepository, ConversationRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IPersonRepository, PersonRepository>();
        services.AddScoped<IRecurringScheduleRepository, RecurringScheduleRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<ISpecialScheduleRepository, SpecialScheduleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    // public static IServiceCollection AddErrors(this IServiceCollection services, Assembly assembly)
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
        services.AddScoped<IBarberShopErrors, BarberShopErrors>();
        services.AddScoped<IConversationErrors, ConversationErrors>();
        services.AddScoped<IMessageErrors, MessageErrors>();
        services.AddScoped<IPersonErrors, PersonErrors>();
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
        services.AddValidatorsFromAssemblyContaining<BarberShopDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<MessageDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<PersonDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<RecurringScheduleDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ReportDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ServiceDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SpecialScheduleDtoRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UserDtoLoginRequestValidator>();

        return services;
    }
}
