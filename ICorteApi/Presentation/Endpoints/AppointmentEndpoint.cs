using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;
using FluentValidation;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Presentation.Endpoints;

public static class AppointmentEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.BarberShop + "/{barberShopId}/" + EndpointPrefixes.Appointment;
    private static readonly string ENDPOINT_NAME = EndpointNames.Appointment;

    public static IEndpointRouteBuilder MapAppointmentEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("{id}", GetAppointment);
        group.MapPost(INDEX, CreateAppointment);
        group.MapPut("{id}", UpdateAppointment);
        group.MapDelete("{id}", DeleteAppointment);

        return app;
    }
    
    public static IResult GetCreatedResult(int newId, int barberShopId)
    {
        string uri = EndpointPrefixes.BarberShop + "/" + barberShopId + "/" + EndpointPrefixes.Appointment + "/" + newId;
        object value = new { Message = "Agendamento criado com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> GetAppointment(
        int id,
        IAppointmentService service,
        IAppointmentErrors errors)
    {
        var res = await service.GetByIdAsync(id);

        if (!res.IsSuccess)
            errors.ThrowNotFoundException();

        var address = res.Value!;
        
        var addressDto = address.CreateDto();
        return Results.Ok(addressDto);
    }

    public static async Task<IResult> CreateAppointment(
        int barberShopId,
        AppointmentDtoRequest dto,
        IValidator<AppointmentDtoRequest> validator,
        IAppointmentService service,
        IUserService userService,
        IAppointmentErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        int clientId = (await userService.GetMeAsync()).Value!.Id;
        var response = await service.CreateAsync(dto, clientId, barberShopId);

        if (!response.IsSuccess)
            errors.ThrowCreateException();

        return GetCreatedResult(response.Value!.Id, barberShopId);
    }

    public static async Task<IResult> UpdateAppointment(
        int id,
        AppointmentDtoRequest dto,
        IValidator<AppointmentDtoRequest> validator,
        IAppointmentService service,
        IAppointmentErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        var response = await service.UpdateAsync(dto, id);

        if (!response.IsSuccess)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAppointment(
        int id,
        IAppointmentService service,
        IAppointmentErrors errors)
    {
        var response = await service.DeleteAsync(id);

        if (!response.IsSuccess)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
