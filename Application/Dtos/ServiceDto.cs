using System.ComponentModel.DataAnnotations;
using ICorteApi.Application.Services;
using ICorteApi.Application.Validators;

namespace ICorteApi.Application.Dtos;

public record ServiceDtoResponse(
    int Id,
    int BarberShopId,
    string? BarberShopName,
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration,
    AppointmentDtoResponse[]? Appointments = null
) : IDtoResponse<Service>
{
    public AppointmentDtoResponse[] Appointments { get; init; } = Appointments ?? [];
};

public record ServiceByNameDtoResponse(
    int Id,
    int BarberShopId,
    string BarberShopName,
    string Name,
    string? Description,
    decimal Price,
    TimeSpan Duration
) : IDtoResponse<Service>;

public record ServiceDtoRequest(
    int BarberShopId,
    string? BarberShopName,

    [Required(ErrorMessage = "Nome obrigatório")]
    [MinLength(3, ErrorMessage = "Nome precisa ter pelo menos 3 caracteres")]
    string Name,

    [Required(ErrorMessage = "Descrição obrigatória")]
    [MinLength(3, ErrorMessage = "Descrição precisa ter pelo menos 3 caracteres")]
    string? Description,

    [Required(ErrorMessage = "Preço obrigatório")]
    [GreaterThan(0, ErrorMessage = "Preço precisa ser maior que R$ 0,00")]
    decimal Price,

    [Required(ErrorMessage = "Duração do serivço não pode estar vazia")]
    TimeSpan Duration
) : IDtoRequest<Service>;

public record ServiceForUpdateAppointmentDtoRequest(
    int Id
) : IDtoRequest<Service>;
