using System.ComponentModel.DataAnnotations;

namespace ICorteApi.Application.Validators;

public abstract class ComparisonAttributeBase : ValidationAttribute
{
    protected ValidationResult AwesomeComparator(
        object? comparisonValue,
        object? value,
        ValidationContext validationContext,
        bool canBeEqual = false)
    {
        ValidationResult GetResult(bool isValid) => isValid
            ? ValidationResult.Success!
            : new ValidationResult(ErrorMessage, [validationContext.MemberName!]);
            
        return (comparisonValue, value) switch
        {
            (DateTime dateTimeValue, DateTime currentValue)
                => GetResult(canBeEqual ? dateTimeValue >= currentValue : dateTimeValue > currentValue),

            (DateOnly dateOnlyValue, DateOnly currentValue)
                => GetResult(canBeEqual ? dateOnlyValue >= currentValue : dateOnlyValue > currentValue),

            (TimeOnly timeOnlyValue, TimeOnly currentValue)
                => GetResult(canBeEqual ? timeOnlyValue >= currentValue : timeOnlyValue > currentValue),

            (TimeSpan timeSpanValue, TimeSpan currentValue)
                => GetResult(canBeEqual ? timeSpanValue >= currentValue : timeSpanValue > currentValue),

            (int intValue, int currentValue)
                => GetResult(canBeEqual ? intValue >= currentValue : intValue > currentValue),

            (decimal decimalValue, decimal currentValue)
                => GetResult(canBeEqual ? decimalValue >= currentValue : decimalValue > currentValue),

            (double doubleValue, double currentValue)
                => GetResult(canBeEqual ? doubleValue >= currentValue : doubleValue > currentValue),

            (float floatValue, float currentValue)
                => GetResult(canBeEqual ? floatValue >= currentValue : floatValue > currentValue),

            _   => throw new ArgumentException("Propriedades precisam ser do mesmo tipo.")
        };
    }
}

public class GreaterThanAttribute(object? valueToCompare) : ComparisonAttributeBase
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        return AwesomeComparator(valueToCompare, value, validationContext);
    }
}

public class GreaterThanOrEqualAttribute(object? valueToCompare) : ComparisonAttributeBase
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        return AwesomeComparator(valueToCompare, value, validationContext, true);
    }
}

public class GreaterThanTodayAttribute() : GreaterThanOrEqualAttribute(DateOnly.FromDateTime(DateTime.UtcNow));

public class GreaterThanOrEqualTodayAttribute() : GreaterThanOrEqualAttribute(DateOnly.FromDateTime(DateTime.UtcNow));

public class GreaterThanPropAttribute(string propName, bool skipIfNull = false) : ComparisonAttributeBase
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var comparisonPropertyInfo = validationContext.ObjectType.GetProperty(propName);

        if (comparisonPropertyInfo is null)
            throw new ArgumentException("Comparison property with this name not found.");

        var comparisonValue = comparisonPropertyInfo.GetValue(validationContext.ObjectInstance);
        
        if (skipIfNull && (comparisonValue is null || value is null))
            return ValidationResult.Success!;
            
        return AwesomeComparator(comparisonValue, value, validationContext);
    }
}

public class GreaterThanOrEqualPropAttribute(string propName, bool skipIfNull = false) : ComparisonAttributeBase
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var comparisonPropertyInfo = validationContext.ObjectType.GetProperty(propName);

        if (comparisonPropertyInfo is null)
            throw new ArgumentException("Comparison property with this name not found.");

        var comparisonValue = comparisonPropertyInfo.GetValue(validationContext.ObjectInstance);
        
        if (skipIfNull && (comparisonValue is null || value is null))
            return ValidationResult.Success!;
            
        return AwesomeComparator(comparisonValue, value, validationContext, true);
    }
}
