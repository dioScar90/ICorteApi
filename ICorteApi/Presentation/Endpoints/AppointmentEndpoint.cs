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
        var appointment = await service.GetByIdAsync(id);

        if (appointment is null)
            errors.ThrowNotFoundException();
        
        return Results.Ok(appointment!.CreateDto());
    }

    // public static async Task<IResult> GetAllAppointments(
    //     int id,
    //     IAppointmentService service,
    //     IAppointmentErrors errors)
    // {
    //     var appointment = await service.GetByIdAsync(id);

    //     if (appointment is null)
    //         errors.ThrowNotFoundException();
        
    //     return Results.Ok(appointment!.CreateDto());
    // }

    public static async Task<IResult> CreateAppointment(
        AppointmentDtoCreate dto,
        IValidator<AppointmentDtoCreate> validator,
        IAppointmentService service,
        IUserService userService,
        IAppointmentErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        int clientId = await userService.GetMyUserIdAsync();
        var appointment = await service.CreateAsync(dto, clientId);

        if (appointment is null)
            errors.ThrowCreateException();

        return GetCreatedResult(appointment!.Id);
    }

    public static async Task<IResult> UpdateAppointment(
        int id,
        AppointmentDtoUpdate dto,
        IValidator<AppointmentDtoUpdate> validator,
        IAppointmentService service,
        IUserService userService,
        IAppointmentErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        int clientId = await userService.GetMyUserIdAsync();
        var result = await service.UpdateAsync(dto, id, clientId);

        if (!result)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteAppointment(
        int id,
        IAppointmentService service,
        IUserService userService,
        IAppointmentErrors errors)
    {
        int clientId = await userService.GetMyUserIdAsync();
        var result = await service.DeleteAsync(id, clientId);

        if (!result)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
