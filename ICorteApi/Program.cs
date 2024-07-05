using ICorteApi.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Identity;
using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Services;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Infraestructure.Repositories;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Domain.Entities;
using ICorteApi.Presentation.Endpoints;
using ICorteApi.Infraestructure.Interceptors;

var builder = WebApplication.CreateBuilder(args);

// builder.Services.AddSingleton<UpdateAuditableInterceptor>();

builder.Services.AddDbContext<IAppDbContext, AppDbContext>((sp, options) =>
    options
        // .AddInterceptors(
        //     sp.GetRequiredService<UpdateAuditableInterceptor>())
        // .UseSqlite(
        //     builder.Configuration.GetConnectionString("SqliteConnection"),
        //     assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
        .UseNpgsql(
            builder.Configuration.GetConnectionString("PostgreSqlConnection"),
            assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
        // .UseInMemoryDatabase("AppDb")
    );

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IBarberShopService, BarberShopService>();
builder.Services.AddScoped<IBarberShopRepository, BarberShopRepository>();
builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAuthorization();

// After .NET 8 we can use AddIdentityApiEndpoints<TUser> and AddRoles<TRole>,
// which is specific for web api applications, instead of AddIdentity<TUser, TRole>.
builder.Services.AddIdentityApiEndpoints<User>(options =>
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

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/auth/login";
    options.LogoutPath = "/auth/logout";
    options.AccessDeniedPath = "/auth/access-denied";
    options.SlidingExpiration = true;
});

// builder.Services.AddAuthentication(options =>
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

// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     // .AddCookie();
//     .AddIdentityCookies();


builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.DefineCultureLocalization("pt-BR");

// Seed dos papéis
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    await RoleSeeder.SeedRoles(roleManager);
}

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

ConfigureEndpoints.MapMyEndpoints(app);

app.MapGet("/", () => "Hello World!");

app.Run();
