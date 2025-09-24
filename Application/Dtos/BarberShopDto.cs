namespace ICorteApi.Application.Dtos;

public record BarberShopDto(
    int Id,
    int OwnerId,
    string Name,
    string? Description,
    string ComercialNumber,
    string ComercialEmail,
    AddressDto? Address,
    RecurringScheduleDto[] RecurringSchedules,
    SpecialScheduleDto[] SpecialSchedules,
    ServiceDto[] Services,
    ReportDto[] Reports
) : IDto<BarberShop>;

public record ClientForAppointmentByBarberShop(
    int Id,
    string FirstName,
    string LastName,
    string FullName
);

public record AppointmentsByBarberShopDto(
    int Id,
    ClientForAppointmentByBarberShop Client,
    int BarberShopId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeSpan TotalDuration,
    string? Notes,
    PaymentType PaymentType,
    decimal TotalPrice,
    ServiceDto[] Services,
    AppointmentStatus Status
) : IDto<BarberShop>;

public record TopBarberShopDto(
    int Id,
    string Name,
    string? Description,
    float Rating
) : IDto<BarberShop>;
