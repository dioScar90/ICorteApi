using System.Collections.Frozen;
using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record OperatingScheduleDtoRequest(
    DayOfWeek DayOfWeek,

    // TimeSpan cannot be directly conversion by JSON until .NET v8
    // string OpenTime,
    // string CloseTime,

    TimeSpan OpenTime,
    TimeSpan CloseTime,

    bool IsActive
) : IDtoRequest;

public record OperatingScheduleDtoResponse(
    DayOfWeek DayOfWeek,
    int BarberShopId,

    TimeSpan OpenTime,
    TimeSpan CloseTime,
    
    bool IsActive
) : IDtoResponse;
