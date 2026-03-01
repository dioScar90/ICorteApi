using System.ComponentModel.DataAnnotations;
using ICorteApi.Application.Services;
using ICorteApi.Application.Validators;

namespace ICorteApi.Application.Dtos;

public record SpecialScheduleDtoResponse(
    DateOnly Date,
    int BarberShopId,
    DayOfWeek DayOfWeek,
    string? Notes = null,
    TimeOnly? OpenTime = null,
    TimeOnly? CloseTime = null,
    bool IsClosed = false
) : IDtoResponse<SpecialSchedule>;

public record SpecialScheduleDtoRequest(
    [Required(ErrorMessage = "Dia obrigatório")]
    DateOnly Date,

    int BarberShopId,
    DayOfWeek DayOfWeek,

    [MinLength(3, ErrorMessage = "Observação, caso informado, precisa ter pelo menos 3 caracteres")]
    string? Notes = null,

    TimeOnly? OpenTime = null,
    
    [GreaterThanProp(nameof(OpenTime), true, ErrorMessage = "Horário de encerramento precisa ser superior ao horário de abertura quando os dois forem informados")]
    TimeOnly? CloseTime = null,

    bool IsClosed = false
) : IDtoRequest<SpecialSchedule>;
