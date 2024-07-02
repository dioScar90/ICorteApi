using ICorteApi.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using ICorteApi.Services;
using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Services;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Infraestructure.Repositories;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Domain.Entities;
using ICorteApi.Presentation.Endpoints;

namespace ICorteApi;

public class Startup(IConfiguration configuration)
{
    private readonly IConfiguration _configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        var connectionString = _configuration.GetConnectionString("SqliteConnection");

        services.AddDbContext<AppDbContext>(options =>
        {
            // options.UseNpgsql(
            //     connectionString,
            //     assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            // );
            options.UseSqlite(
                connectionString,
                assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)
            );
            // options.UseInMemoryDatabase("AppDb");
        });
        
        // services.AddScoped<AppDbContext>();
        services.AddScoped<IBarberShopService, BarberShopService>();
        services.AddScoped<IBarberShopRepository, BarberShopRepository>();
        
        services.AddAuthorization();
        
        // After .NET 8 we can use AddIdentityApiEndpoints<TUser> and AddRoles<TRole>,
        // which is specific for web api applications, instead of AddIdentity<TUser, TRole>.
        services.AddIdentityApiEndpoints<User>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequireUppercase = true;
            options.Password.RequiredLength = 6;
            options.Password.RequiredUniqueChars = 1;

            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;

            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = false;
        })
        .AddRoles<IdentityRole<int>>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();
        
        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            options.LoginPath = "/auth/login";
            options.LogoutPath = "/auth/logout";
            options.AccessDeniedPath = "/auth/access-denied";
            options.SlidingExpiration = true;
        });

        // services.AddAuthentication(options =>
        // {
        //     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        //     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        // })
        // .AddJwtBearer(options =>
        // {
        //     options.RequireHttpsMetadata = false;
        //     options.SaveToken = true;
        //     options.TokenValidationParameters = new TokenValidationParameters
        //     {
        //         ValidateIssuer = true,
        //         ValidateAudience = true,
        //         ValidateLifetime = true,
        //         ValidateIssuerSigningKey = true,
        //         ValidIssuer = _configuration["Jwt:Issuer"],
        //         ValidAudience = _configuration["Jwt:Audience"],
        //         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!))
        //     };
        // });

        // services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        //     // .AddCookie();
        //     .AddIdentityCookies();
        

        services.AddEndpointsApiExplorer();
        
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "BarberShop API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
            }});
        });
    }

    public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.DefineCultureLocalization("pt-BR");

        // Seed dos papéis
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            await RoleSeeder.SeedRoles(roleManager);
        }

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapMyEndpoints();
    }
}
