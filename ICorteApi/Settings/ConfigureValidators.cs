using FluentValidation;
using ICorteApi.Application.Validators;

namespace ICorteApi.Settings;

public static class ConfigureValidators
{
    public static void AddAll(WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<AddressDtoCreateValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<AddressDtoUpdateValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<AppointmentDtoRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<BarberShopDtoRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<MessageDtoRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<ProfileDtoUpdateValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<ProfileDtoCreateValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<RecurringScheduleDtoRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<ReportDtoRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<ServiceDtoRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<SpecialScheduleDtoRequestValidator>();
        builder.Services.AddValidatorsFromAssemblyContaining<UserDtoLoginRequestValidator>();
    }
}
