using System.Net;
using FluentValidation;
using ICorteApi.Application.Services;
using ICorteApi.Application.Validators;
using ICorteApi.Domain.Errors;
using ICorteApi.Settings;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<AdminService>();
        services.AddScoped<AddressService>();
        services.AddScoped<AppointmentService>();
        services.AddScoped<BarberScheduleService>();
        services.AddScoped<BarberShopService>();
        services.AddScoped<MessageService>();
        services.AddScoped<ProfileService>();
        services.AddScoped<RecurringScheduleService>();
        services.AddScoped<ReportService>();
        services.AddScoped<ServiceService>();
        services.AddScoped<SpecialScheduleService>();
        services.AddScoped<UserService>();

        return services;
    }

    public static IServiceCollection AddErrors(this IServiceCollection services)
    {
        services.AddScoped<AdminErrors>();
        services.AddScoped<AddressErrors>();
        services.AddScoped<AppointmentErrors>();
        services.AddScoped<ProfileErrors>();
        services.AddScoped<BarberShopErrors>();
        services.AddScoped<ImageErrors>();
        services.AddScoped<MessageErrors>();
        services.AddScoped<RecurringScheduleErrors>();
        services.AddScoped<ReportErrors>();
        services.AddScoped<ServiceErrors>();
        services.AddScoped<SpecialScheduleErrors>();
        services.AddScoped<UserErrors>();

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<AddressValidator>();
        services.AddValidatorsFromAssemblyContaining<AppointmentValidator>();
        services.AddValidatorsFromAssemblyContaining<BarberShopValidator>();
        services.AddValidatorsFromAssemblyContaining<MessageValidator>();
        services.AddValidatorsFromAssemblyContaining<ProfileValidator>();
        services.AddValidatorsFromAssemblyContaining<RecurringScheduleValidator>();
        services.AddValidatorsFromAssemblyContaining<ReportValidator>();
        services.AddValidatorsFromAssemblyContaining<ServiceValidator>();
        services.AddValidatorsFromAssemblyContaining<SpecialScheduleValidator>();
        services.AddValidatorsFromAssemblyContaining<UserValidator>();
        services.AddValidatorsFromAssemblyContaining<UserDtoLoginRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UserDtoEmailUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<UserDtoPasswordUpdateValidator>();
        services.AddValidatorsFromAssemblyContaining<UserDtoPhoneNumberUpdateValidator>();

        return services;
    }

    public static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        // After .NET 8 we can use IExceptionHandler interface
        services.AddProblemDetails(configure =>
        {
            configure.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);
            };
        });
        services.AddExceptionHandler<GlobalExceptionHandler>();

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

    public static IServiceCollection AddCookieConfiguration(this IServiceCollection services, bool isDevelopment)
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

            if (isDevelopment)
            {
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
            }
            else
            {
                options.Cookie.SameSite = SameSiteMode.None; // Permite cookies em requisições de diferentes origens
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Requer HTTPS

            }
            
            // Útil para prolongar a sessão ativa se o usuário estiver ativo.
            options.SlidingExpiration = true;

            // Define o tempo de vida do cookie de autenticação, ou seja, quanto tempo o cookie permanece válido antes de expirar.
            options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

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
        });

        return services;
    }
    
    public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, bool isDevelopment)
    {
        services.AddCors(options =>
        {
            if (isDevelopment)
            {
                var hostAddresses = Dns.GetHostAddresses(Dns.GetHostName());
                var ipv4 = hostAddresses.FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);

                string[] ipOrigin = ipv4 is null ? [] : [$"http://{ipv4}:5173"];
                string[] localhostOrigin = ["http://localhost:5173"];
                
                HashSet<string> origins = [..ipOrigin, ..localhostOrigin];
                
                foreach (var suamae in origins)
                {
                    Console.WriteLine($"\n\n\nToma aqui seu IP => {suamae}");
                }
                
                options.AddPolicy("AllowSpecificOrigin", policy =>
                {
                    policy.WithOrigins([..origins])
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // Permite o uso de cookies ou cabeçalhos de autenticação
                });
            }
            else
            {
                // Configuração mais restritiva em produção
                options.AddPolicy("ProductionPolicy", policy =>
                {
                    policy.WithOrigins("https://dioscar90.github.io", "https://icorte.netlify.app") // Domínios
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // Permitir envio de cookies
                });
            }
        });

        return services;
    }
}
