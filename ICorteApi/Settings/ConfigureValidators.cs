using FluentValidation;
using ICorteApi.Application.Validators;

namespace ICorteApi.Settings;

public static class ConfigureValidators
{
    public static void AddAll(WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<AddressDtoRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UserDtoLoginRequestValidator>();
    }
}
