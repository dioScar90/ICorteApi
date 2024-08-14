using FluentValidation;
using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Services;
using ICorteApi.Application.Validators;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Enums;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Infraestructure.Repositories;
using ICorteApi.Presentation.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
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

    public static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        // After .NET 8 we can use IExceptionHandler interface
        services.AddExceptionHandler<ExceptionHandler>();
        services.AddProblemDetails();

        return services;
    }

    public static IServiceCollection AddAuthorizationRules(this IServiceCollection services)
    {
        // Configuração de autenticação e autorização
        // After .NET 8 it's not necessary to use `AddAuthentication` here.
        services.AddAuthorizationBuilder()
            
            .AddPolicy(nameof(PolicyUserRole.Free), policy =>
                policy.RequireRole(
                    nameof(UserRole.Guest), nameof(UserRole.Client), nameof(UserRole.BarberShop), nameof(UserRole.Admin)))
            
            .AddPolicy(nameof(PolicyUserRole.ClientOrHigh), policy =>
                policy.RequireRole(
                    nameof(UserRole.Client), nameof(UserRole.BarberShop), nameof(UserRole.Admin)))
            
            .AddPolicy(nameof(PolicyUserRole.ClientOnly), policy =>
                policy.RequireRole(
                    nameof(UserRole.Client), nameof(UserRole.Admin)))
            
            .AddPolicy(nameof(PolicyUserRole.BarberShopOrHigh), policy =>
                policy.RequireRole(
                    nameof(UserRole.BarberShop), nameof(UserRole.Admin)))
                    
            .AddPolicy(nameof(PolicyUserRole.AdminOnly), policy =>
                policy.RequireRole(
                    nameof(UserRole.Admin)));
        
        return services;
    }

    public static IServiceCollection AddIdentityConfigurations(this IServiceCollection services)
    {
        services.AddIdentityApiEndpoints<User>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;

                // Define o tempo de bloqueio da conta de um usuário após várias tentativas de login fracassadas.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;

                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                // options.User.AllowedUserNameCharacters = "a-zA-Z0-9-._@#$+";
                options.User.RequireUniqueEmail = true; // Ajustado para exigir e-mails únicos
            })
            .AddRoles<IdentityRole<int>>()
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
            
        return services;
    }

    public static IServiceCollection AddCookieConfiguration(this IServiceCollection services)
    {
        // Configuração de autenticação por cookies
        services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/auth/login";
                options.LogoutPath = "/auth/logout";
                options.AccessDeniedPath = "/auth/access-denied";
                
                // Útil para prolongar a sessão ativa se o usuário estiver ativo.
                options.SlidingExpiration = true;
                
                // Garante que os cookies sejam enviados apenas em conexões HTTPS, o que é ótimo para segurança.
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                
                // Define o tempo de vida do cookie de autenticação, ou seja, quanto tempo o cookie permanece válido antes de expirar.
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            });
            
        return services;
    }

    public static IServiceCollection AddAntiCsrfConfiguration(this IServiceCollection services)
    {
        // Configuração de proteção contra ataques de Cross-Site Request Forgery (CSRF).
        services.AddAntiforgery(options =>
        {
            /*
            Frontend:
                Ao enviar uma requisição que pode alterar dados (como uma requisição POST), o token CSRF
                deve ser incluído no cabeçalho da requisição com o nome "X-CSRF-TOKEN". Isso é geralmente
                feito em requisições AJAX, onde você pode configurar o cliente HTTP (por exemplo, Axios,
                Fetch API) para incluir o token no cabeçalho.
            Backend:
                No servidor, o ASP.NET Core verifica a presença do token no cabeçalho "X-CSRF-TOKEN" e
                valida seu valor. Se o token estiver ausente ou for inválido, a requisição é rejeitada,
                protegendo a aplicação contra ataques CSRF.
            */
            options.HeaderName = "X-CSRF-TOKEN";
        });

        return services;
    }
}
