using System.ComponentModel.DataAnnotations;

namespace ICorteApi.Application.Validators;

public class DayOfWeekValidatorAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) => value is int and >= 0 and <= 6;
}
