using System.ComponentModel.DataAnnotations;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Validators;

public class PreventNullAfterSetAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var entity = validationContext.ObjectInstance as User;

        if (entity != null && value == null)
        {
            // Get the property name that this attribute is applied to
            var propertyName = validationContext.MemberName;
            var property = entity.GetType().GetProperty(propertyName);

            // Check if the property already has a value
            if (property != null)
            {
                var currentValue = property.GetValue(entity);

                if (currentValue != null)
                    return new ValidationResult($"{propertyName} cannot be set to null after it has been assigned a value.");
            }
        }

        return ValidationResult.Success;
    }
}
