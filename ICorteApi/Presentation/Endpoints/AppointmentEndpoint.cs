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
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.Appointment;
    private static readonly string ENDPOINT_NAME = EndpointNames.Appointment;

    public static IEndpointRouteBuilder MapAppointmentEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPost(INDEX, CreateAppointment);
        group.MapGet("{id}", GetAppointment);
        // group.MapGet(INDEX, GetAllAppointments);
        group.MapPut("{id}", UpdateAppointment);
        group.MapDelete("{id}", DeleteAppointment);

        return app;
    }
    
    public static IResult GetCreatedResult(int newId)
    {
        string uri = EndpointPrefixes.Appointment + "/" + newId;
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
        AppointmentDtoRequest dto,
        IValidator<AppointmentDtoRequest> validator,
        IAppointmentService service,
        IUserService userService,
        IAppointmentErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        int clientId = (await userService.GetMeAsync()).Value!.Id;
        var response = await service.CreateAsync(dto, clientId);

        if (!response.IsSuccess)
            errors.ThrowCreateException(response.Error);

        return GetCreatedResult(response.Value!.Id);
    }

    public static async Task<IResult> UpdateAppointment(
        int id,
        AppointmentDtoRequest dto,
        IValidator<AppointmentDtoRequest> validator,
        IAppointmentService service,
        IUserService userService,
        IAppointmentErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        int clientId = (await userService.GetMeAsync()).Value!.Id;
        var response = await service.UpdateAsync(dto, id, clientId);

        if (!response.IsSuccess)
            errors.ThrowUpdateException(response.Error);

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAppointment(
        int id,
        IAppointmentService service,
        IUserService userService,
        IAppointmentErrors errors)
    {
        int clientId = (await userService.GetMeAsync()).Value!.Id;
        var response = await service.DeleteAsync(id, clientId);

        if (!response.IsSuccess)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
