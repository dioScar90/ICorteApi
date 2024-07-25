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
using ICorteApi.Presentation.Exceptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options
        // .UseSqlite(
        //     builder.Configuration.GetConnectionString("SqliteConnection"),
        //     assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
        .UseNpgsql(
            builder.Configuration.GetConnectionString("PostgreSqlConnection"),
            assembly => assembly.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName))
    // .UseInMemoryDatabase("AppDb")
    );

builder.Services.AddHttpContextAccessor();

// builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
// builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
// builder.Services.AddScoped<IAppointmentServiceService, AppointmentServiceService>();
builder.Services.AddScoped<IAppointmentServiceRepository, AppointmentServiceRepository>();
// builder.Services.AddScoped<IBarberShopService, BarberShopService>();
builder.Services.AddScoped<IBarberShopRepository, BarberShopRepository>();
// builder.Services.AddScoped<IConversationParticipantService, ConversationParticipantService>();
builder.Services.AddScoped<IConversationParticipantRepository, ConversationParticipantRepository>();
// builder.Services.AddScoped<IConversationService, ConversationService>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();
// builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
// builder.Services.AddScoped<IPersonService, PersonService>();
builder.Services.AddScoped<IPersonRepository, PersonRepository>();
// builder.Services.AddScoped<IRecurringScheduleService, RecurringScheduleService>();
builder.Services.AddScoped<IRecurringScheduleRepository, RecurringScheduleRepository>();
// builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();
// builder.Services.AddScoped<IServiceService, ServiceService>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
// builder.Services.AddScoped<ISpecialScheduleService, SpecialScheduleService>();
builder.Services.AddScoped<ISpecialScheduleRepository, SpecialScheduleRepository>();
// builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register validator with service provider (or use one of the automatic registration methods)
ConfigureValidators.AddAll(builder);

// After .NET 8 we can use IExceptionHandler interface
builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();

// After .NET 8 it's not necessary to use `AddAuthentication` here.
builder.Services.AddAuthorization();

// After .NET 8 we can use AddIdentityApiEndpoints<TUser> and AddRoles<TRole>,
// which is specific for web api applications, instead of AddIdentity<TUser, TRole>.
builder.Services
.AddIdentityApiEndpoints<User>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;

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
        []
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

app.UseExceptionHandler();

app.Run();
