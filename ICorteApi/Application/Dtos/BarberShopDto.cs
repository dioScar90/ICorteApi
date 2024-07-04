using System.Collections.Frozen;
using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record BarberShopDtoRequest(
    string Name,
    string? Description,
    string ComercialNumber,
    string ComercialEmail,
    // string OperatingSchedule, // TimeSpan cannot be directly conversion by JSON until .NET v8
    // string ClosingHours, // TimeSpan cannot be directly conversion by JSON until .NET v8
    // string DaysOpen,
    double? Rating,
    AddressDtoRequest? Address,
    OperatingScheduleDtoRequest[]? OperatingSchedules,
    PersonDtoRequest[]? Barbers
// Schedule[]? Schedules,
// Service[]? Services
) : IDtoRequest;

public record BarberShopDtoResponse(
    int Id,
    string Name,
    string? Description,
    string ComercialNumber,
    string ComercialEmail,
    double? Rating,

    AddressDtoResponse? Address,
    OperatingScheduleDtoResponse[]? OperatingSchedules,
    PersonDtoResponse[]? Barbers
// Schedule[]? Schedules,
// Service[]? Services
) : IDtoResponse;
