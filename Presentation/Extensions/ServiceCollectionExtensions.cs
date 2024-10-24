using FluentValidation;
using ICorteApi.Application.Services;
using ICorteApi.Application.Validators;
using ICorteApi.Domain.Errors;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Settings;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAdminRepository, AdminRepository>();
        services.AddScoped<IAddressRepository, AddressRepository>();
        services.AddScoped<IAppointmentRepository, AppointmentRepository>();
        services.AddScoped<IBarberScheduleRepository, BarberScheduleRepository>();
        services.AddScoped<IBarberShopRepository, BarberShopRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IRecurringScheduleRepository, RecurringScheduleRepository>();
        services.AddScoped<IReportRepository, ReportRepository>();
        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<ISpecialScheduleRepository, SpecialScheduleRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IBarberScheduleService, BarberScheduleService>();
        services.AddScoped<IBarberShopService, BarberShopService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IProfileService, ProfileService>();
        services.AddScoped<IRecurringScheduleService, RecurringScheduleService>();
        services.AddScoped<IReportService, ReportService>();
        services.AddScoped<IServiceService, ServiceService>();
        services.AddScoped<ISpecialScheduleService, SpecialScheduleService>();
        services.AddScoped<IUserService, UserService>();

        return services;
    }

    public static IServiceCollection AddErrors(this IServiceCollection services)
    {
        services.AddScoped<IAdminErrors, AdminErrors>();
        services.AddScoped<IAddressErrors, AddressErrors>();
        services.AddScoped<IAppointmentErrors, AppointmentErrors>();
        services.AddScoped<IProfileErrors, ProfileErrors>();
        services.AddScoped<IBarberShopErrors, BarberShopErrors>();
        services.AddScoped<IImageErrors, ImageErrors>();
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
        services.AddValidatorsFromAssemblyContaining<AddressDtoCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<AddressDtoUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<AppointmentDtoCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<AppointmentDtoUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<BarberShopDtoCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<BarberShopDtoUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<MessageDtoCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<MessageDtoIsReadUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<ProfileDtoCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<ProfileDtoUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<RecurringScheduleDtoCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<RecurringScheduleDtoUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<ReportDtoCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<ReportDtoUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<ServiceDtoCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<ServiceDtoUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<SpecialScheduleDtoCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<SpecialScheduleDtoUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<UserDtoLoginRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UserDtoRegisterCreateValidator>();
        services.AddValidatorsFromAssemblyContaining<UserDtoEmailUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<UserDtoPasswordUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<UserDtoPhoneNumberUpdateValidator>();

        return services;
    }

    public static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        // After .NET 8 we can use IExceptionHandler interface
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        // // Se estiver usando .NET 8 ou superior
        // services.AddExceptionHandler<GlobalExceptionHandler>();

        // // Adiciona suporte para detalhes de problemas HTTP
        // services.AddProblemDetails(options =>
        // {
        //     options.CustomizeProblemDetails = (con)
        //     options.IncludeExceptionDetailInProblemDetails = (context, exception) =>
        //         context.RequestServices.GetRequiredService<IWebHostEnvironment>().IsDevelopment();

        //     options.Map<UnauthorizedAccessException>(ex => new ProblemDetails
        //     {
        //         Status = StatusCodes.Status403Forbidden,
        //         Title = "Forbidden",
        //         Detail = ex.Message
        //     });

        //     options.Map<NotFoundException>(ex => new ProblemDetails
        //     {
        //         Status = StatusCodes.Status404NotFound,
        //         Title = "Not Found",
        //         Detail = ex.Message
        //     });

        //     // Adicione mapeamentos personalizados conforme necessário
        // });

        return services;
    }

    public static IServiceCollection AddAuthorizationRules(this IServiceCollection services)
    {
        // Configuração de autenticação e autorização
        // After .NET 8 it's not necessary to use `AddAuthentication` here.
        // The use of `AddAuthorization` can be converted to the new `AddAuthorizationBuilder`.
        // https://learn.microsoft.com/en-us/aspnet/core/diagnostics/asp0025?view=aspnetcore-8.0
        services.AddAuthorizationBuilder()
            .AddPolicy(nameof(PolicyUserRole.AdminOnly), policy =>
                policy.RequireRole(
                    nameof(UserRole.Admin)))
            .AddPolicy(nameof(PolicyUserRole.BarberShopOrHigh), policy =>
                policy.RequireRole(
                    nameof(UserRole.BarberShop), nameof(UserRole.Admin)))
            .AddPolicy(nameof(PolicyUserRole.ClientOnly), policy =>
                policy.RequireRole(
                    nameof(UserRole.Client), nameof(UserRole.Admin)))
            .AddPolicy(nameof(PolicyUserRole.ClientOrHigh), policy =>
                policy.RequireRole(
                    nameof(UserRole.Client), nameof(UserRole.BarberShop), nameof(UserRole.Admin)))
            .AddPolicy(nameof(PolicyUserRole.FreeIfAuthenticated), policy =>
                policy.RequireRole(
                    nameof(UserRole.Guest), nameof(UserRole.Client), nameof(UserRole.BarberShop), nameof(UserRole.Admin)));

        return services;
    }

    public static IServiceCollection AddIdentityConfigurations(this IServiceCollection services)
    {
        services.AddIdentity<User, ApplicationRole>(options =>
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
                options.User.RequireUniqueEmail = true; // Ajustado para exigir e-mails únicos

                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
            })
            .AddEntityFrameworkStores<AppDbContext>()

            // This `AddDefaultUI` above is necessary to not display
            // `No Registered Service for IEmailSender` error message
            // after run the application by `dotnet run`
            .AddDefaultUI()

            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddCookieConfiguration(this IServiceCollection services)
    {
        // Configuração de autenticação por cookies
        services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnSigningIn = context =>
                {
                    var currentToken = SessionTokenManager.GetCurrentToken();
                    context.Properties.Items["SessionToken"] = currentToken;
                    return Task.CompletedTask;
                };

                options.Events.OnValidatePrincipal = context =>
                {
                    if (context.Properties.Items.TryGetValue("SessionToken", out var sessionToken))
                    {
                        if (sessionToken != SessionTokenManager.GetCurrentToken())
                        {
                            context.RejectPrincipal(); // Rejeitar o principal se o token não corresponder
                        }
                    }
                    return Task.CompletedTask;
                };

                options.Cookie.HttpOnly = true;

                options.LoginPath = "/auth/login";
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    return Task.CompletedTask;
                };

                options.LogoutPath = "/auth/logout";

                options.AccessDeniedPath = "/auth/access-denied";
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    return Task.CompletedTask;
                };

                // Útil para prolongar a sessão ativa se o usuário estiver ativo.
                options.SlidingExpiration = true;

                // Garante que os cookies sejam enviados apenas em conexões HTTPS, o que é ótimo para segurança.
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                // Define o tempo de vida do cookie de autenticação, ou seja, quanto tempo o cookie permanece válido antes de expirar.
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
            });

        return services;
    }

    public static IServiceCollection AddCustomDataProtection(this IServiceCollection services)
    {
        _ = services.AddDataProtection()
            .SetApplicationName("ICorteApi")
            .PersistKeysToFileSystem(new DirectoryInfo(@"./keys")) // Local onde as chaves são armazenadas
            .ProtectKeysWithDpapi(); // Proteção adicional para as chaves

        return services;
    }

    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            // options.AddPolicy("AllowSpecificOrigin",
            options.AddPolicy("AllowAll",
                builder => builder
                    .WithOrigins() // This way any origin is allowed. Beware in production.
                    // .WithOrigins("http://localhost:5173") // This must be preferable overrided by env variable
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()); // Permite o uso de cookies ou cabeçalhos de autenticação
        });

        return services;
    }
}
