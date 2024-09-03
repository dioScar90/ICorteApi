using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Dtos;

public record SpecialScheduleDtoRequest : IDtoRequest<SpecialSchedule>
{
    public SpecialScheduleDtoRequest(DateOnly date, string? notes, TimeOnly? openTime, TimeOnly? closeTime, bool isClosed)
    {
        Date = date;
        Notes = notes;
        OpenTime = isClosed ? null : openTime;
        CloseTime = isClosed ? null : closeTime;
        IsClosed = isClosed || (openTime is null && closeTime is null);
    }
    
    public DateOnly Date { get; init; }
    public string? Notes { get; init; }
    public TimeOnly? OpenTime { get; init; }
    public TimeOnly? CloseTime { get; init; }
    public bool IsClosed { get; init; }
}

public record SpecialScheduleDtoResponse(
    DateOnly Date,
    int BarberShopId,
    string? Notes,
    TimeOnly? OpenTime,
    TimeOnly? CloseTime,
    bool IsClosed
) : IDtoResponse<SpecialSchedule>;
