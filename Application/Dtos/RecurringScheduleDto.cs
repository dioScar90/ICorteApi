using System.ComponentModel.DataAnnotations;
using ICorteApi.Application.Services;
using ICorteApi.Application.Validators;

namespace ICorteApi.Application.Dtos;

public record RecurringScheduleDtoResponse(
    DayOfWeek DayOfWeek,
    int BarberShopId,
    TimeOnly OpenTime,
    TimeOnly CloseTime,
    bool IsActive
) : IDtoResponse<RecurringSchedule>;

public record RecurringScheduleDtoRequest(
    [Required(ErrorMessage = "Dia da semana obrigatório")]
    [EnumDataType(typeof(DayOfWeek), ErrorMessage = "Dia da semana inválido")]
    DayOfWeek DayOfWeek,

    int BarberShopId,
    TimeOnly OpenTime,
    
    [GreaterThanProp(nameof(OpenTime), ErrorMessage = "Horário de encerramento precisa ser superior ao horário de abertura")]
    TimeOnly CloseTime,

    bool IsActive
) : IDtoRequest<RecurringSchedule>;
