using System.ComponentModel.DataAnnotations;
using ICorteApi.Application.Services;
using ICorteApi.Application.Validators;

namespace ICorteApi.Application.Dtos;

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

public record BarberShopDtoRequest(
    int Id,
    int OwnerId,

    [Required(ErrorMessage = "Nome obrigatório")]
    [MinLength(3, ErrorMessage = "Nome precisa ter pelo menos 3 caracteres")]
    string Name,
    
    [MinLength(3, ErrorMessage = "Descrição precisa ter pelo menos 3 caracteres")]
    string? Description,
    
    [Required(ErrorMessage = "Telefone comercial obrigatório")]
    [PhoneNumber]
    string ComercialNumber,
    
    [Required(ErrorMessage = "Email obrigatório")]
    [RegularExpression(@".$", ErrorMessage = "Email incompleto ou com formato inválido")]
    [EmailAddress(ErrorMessage = "Email com formato inválido")]
    string ComercialEmail,
    
    AddressDtoRequest? Address,
    RecurringScheduleDtoRequest[] RecurringSchedules,
    SpecialScheduleDtoRequest[] SpecialSchedules,
    ServiceDtoRequest[] Services,
    ReportDtoRequest[] Reports
) : IDtoRequest<BarberShop>;

public record BarberShopDtoForDataSeederRequest(
    [Required(ErrorMessage = "Nome obrigatório")]
    [MinLength(3, ErrorMessage = "Nome precisa ter pelo menos 3 caracteres")]
    string Name,
    
    [MinLength(3, ErrorMessage = "Descrição precisa ter pelo menos 3 caracteres")]
    string? Description,
    
    [Required(ErrorMessage = "Telefone comercial obrigatório")]
    [PhoneNumber]
    string ComercialNumber,
    
    [Required(ErrorMessage = "Email obrigatório")]
    [RegularExpression(@".$", ErrorMessage = "Email incompleto ou com formato inválido")]
    [EmailAddress(ErrorMessage = "Email com formato inválido")]
    string ComercialEmail,
    
    AddressDtoRequest? Address,
    RecurringScheduleDtoRequest[] RecurringSchedules,
    SpecialScheduleDtoRequest[] SpecialSchedules,
    ServiceDtoRequest[] Services,
    ReportDtoRequest[] Reports
) : IDtoRequest<BarberShop>;

public record Aopa(int Id,
    string Name,
    string? Description,
    string ComercialNumber,
    string ComercialEmail,
    AddressDtoRequest? Address,
    RecurringScheduleDtoRequest[] RecurringSchedules,
    SpecialScheduleDtoRequest[] SpecialSchedules,
    ServiceDtoRequest[] Services,
    ReportDtoRequest[] Reports
) : BarberShopDtoForDataSeederRequest(
    Name,
    Description,
    ComercialNumber,
    ComercialEmail,
    Address,
    RecurringSchedules,
    SpecialSchedules,
    Services,
    Reports
);

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
