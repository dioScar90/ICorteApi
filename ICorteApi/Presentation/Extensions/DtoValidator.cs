using FluentValidation;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Presentation.Extensions;

public static class DtoValidator
{
    public static void CheckAndThrowExceptionIfInvalid<TDto, TEntity>(this TDto dto, IValidator<TDto> validator, IBaseErrors<TEntity> errors)
        where TEntity : class, IBaseTableEntity
        where TDto : IDtoRequest<TEntity>
    {
        var validationResult = validator.Validate(dto);
        
        if (!validationResult.IsValid)
            errors.ThrowValidationException(validationResult.ToDictionary());
    }
}
