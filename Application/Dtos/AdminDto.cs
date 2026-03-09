using System.ComponentModel.DataAnnotations;
using ICorteApi.Application.Services;
using ICorteApi.Application.Validators;

namespace ICorteApi.Application.Dtos;

public record ResetPasswordDto(
    [Required(ErrorMessage = "Email obrigatório")]
    [Email]
    string Email
) : IDtoRequest;

public record PeriodToPopulateDto : IDtoRequest
{
    public DateOnly DayToPopulate { get; init; }
    public DateOnly VeryLimitDate { get; init; }
    
    public PeriodToPopulateDto(DateOnly? firstDate, DateOnly? limitDate)
    {
        DayToPopulate = firstDate ?? DateOnly.FromDateTime(DateTime.Now);
        VeryLimitDate = limitDate ?? DayToPopulate.AddDays(30);
    }
}
 