namespace ICorteApi.Application.Dtos;

public record BarberShopDtoCreate(
    string Name,
    string? Description,
    string ComercialNumber,
    string ComercialEmail,
    AddressDtoCreate? Address = null,
    RecurringScheduleDtoCreate[]? RecurringSchedules = null,
    SpecialScheduleDtoCreate[]? SpecialSchedules = null,
    ServiceDtoCreate[]? Services = null
) : IDtoRequest<BarberShop>;

public record BarberShopDtoUpdate(
    string Name,
    string? Description,
    string ComercialNumber,
    string ComercialEmail,
    AddressDtoUpdate? Address = null
) : IDtoRequest<BarberShop>;

public record BarberShopDtoResponse(
    int Id,
    int OwnerId,
    string Name,
    string? Description,
    string ComercialNumber,
    string ComercialEmail,
    AddressDtoResponse? Address,
    RecurringScheduleDtoResponse[] RecurringSchedules,
    SpecialScheduleDtoResponse[] SpecialSchedules,
    ServiceDtoResponse[] Services,
    ReportDtoResponse[] Reports
) : IDtoResponse<BarberShop>;

public record ClientForAppointmentByBarberShop(
    int Id,
    string FirstName,
    string LastName,
    string FullName
);

public record AppointmentsByBarberShopDtoResponse(
    int Id,
    ClientForAppointmentByBarberShop Client,
    int BarberShopId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeSpan TotalDuration,
    string? Notes,
    PaymentType PaymentType,
    decimal TotalPrice,
    ServiceDtoResponse[] Services,
    AppointmentStatus Status
) : IDtoResponse<BarberShop>;

public record TopBarberShopDtoResponse(
    int Id,
    string Name,
    string? Description,
    float Rating
) : IDtoResponse<BarberShop>;
