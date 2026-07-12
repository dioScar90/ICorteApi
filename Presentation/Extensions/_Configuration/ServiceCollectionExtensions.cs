using System.Net;
using ICorteApi.Application.Services;
using ICorteApi.Domain.Errors;
using ICorteApi.Settings;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddServices()
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
        
        public IServiceCollection AddErrors()
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
        
        public IServiceCollection AddValidators()
        {
            services.AddValidators();
            
            return services;
        }
        
        public IServiceCollection AddExceptionHandlers()
        {
            // After .NET 8 we can use IExceptionHandler interface
            services.AddProblemDetails(configure =>
            {
                configure.CustomizeProblemDetails = context =>
                {
                    context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                    if (context.ProblemDetails is HttpValidationProblemDetails validation)
                    {
                        context.HttpContext.RequestServices
                            .GetRequiredService<ILoggerFactory>()
                            .CreateLogger("Validation")
                            .LogInformation(
                                "Validation failed. Path={Path} Errors={@Errors} TraceId={TraceIdentifier}",
                                context.HttpContext.Request.Path,
                                validation.Errors,
                                context.HttpContext.TraceIdentifier);
                            
                        context.ProblemDetails.Title = "Validation failed";
                        context.ProblemDetails.Type  = "https://example.com/problems/validation";
                        context.ProblemDetails.Extensions["traceId"] = context.HttpContext.TraceIdentifier;
                        
                        context.ProblemDetails.Extensions["errors"] = validation.Errors.ToDictionary(
                            kvp => kvp.Key,
                            kvp => (object)kvp.Value
                        );
                    }
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

        public IServiceCollection AddAuthorizationRules()
        {
            // Configuração de autenticação e autorização
            // After .NET 8 it's not necessary to use `AddAuthentication` here.
            // The use of `AddAuthorization` can be converted to the new `AddAuthorizationBuilder`.
            // https://learn.microsoft.com/en-us/aspnet/core/diagnostics/asp0025?view=aspnetcore-8.0

            var authBuilder = services.AddAuthorizationBuilder();
            
            foreach (var policyRole in Enum.GetValues<PolicyUserRole>())
            {
                string name = policyRole.ToString();
                string[] roles = policyRole.GetRolesString();

                if (policyRole == PolicyUserRole.FreeIfAuthenticated)
                    authBuilder.AddDefaultPolicy(name, policy => policy.RequireRole(roles));
                else
                    authBuilder.AddPolicy(name, policy => policy.RequireRole(roles));
            }
            
            return services;
        }

        public IServiceCollection AddIdentityConfigurations()
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

        public IServiceCollection AddCookieConfiguration(bool isDevelopment)
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
        
        public IServiceCollection AddCorsConfiguration(bool isDevelopment)
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
}
