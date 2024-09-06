using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;
using ICorteApi.Domain.Interfaces;
using FluentValidation;
using ICorteApi.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class ChatEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.Appointment + "/{appointmentId}/" + EndpointPrefixes.Chat;
    private static readonly string ENDPOINT_NAME = EndpointNames.Chat;

    public static IEndpointRouteBuilder MapMessageEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet(INDEX, GetAllMessages);
        group.MapGet("{id}", GetMessage);
        group.MapPost(INDEX, CreateMessage);
        group.MapDelete("{id}", DeleteMessage);

        return app;
    }

    public static IResult GetCreatedResult(int newId, int appointmentId)
    {
        string uri = EndpointPrefixes.Appointment + "/" + appointmentId + "/" + EndpointPrefixes.Chat + "/" + newId;
        object value = new { Message = "Mensagem enviada com sucesso" };
        return Results.Created(uri, value);
    }

    public static async Task<IResult> GetMessage(
        int id,
        int appointmentId,
        IMessageService service,
        IMessageErrors errors)
    {
        var message = await service.GetByIdAsync(id, appointmentId);

        if (message is null)
            errors.ThrowNotFoundException();

        return Results.Ok(message!.CreateDto());
    }

    public static async Task<IResult> GetAllMessages(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int appointmentId,
        IMessageService service,
        IMessageErrors errors)
    {
        var messages = await service.GetAllAsync(page, pageSize, appointmentId);

        var dtos = messages?.Select(m => m.CreateDto()).ToArray() ?? [];
        return Results.Ok(dtos);
    }

    public static async Task<IResult> CreateMessage(
        int appointmentId,
        MessageDtoRequest dto,
        IValidator<MessageDtoRequest> validator,
        IMessageService service,
        IUserService userService,
        IMessageErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);

        int senderId = await userService.GetMyUserIdAsync();
        var message = await service.CreateAsync(dto, appointmentId, senderId);

        if (message is null)
            errors.ThrowCreateException();

        return GetCreatedResult(message!.Id, appointmentId);
    }

    public static async Task<IResult> DeleteMessage(
        int appointmentId,
        int id,
        IMessageService service,
        IMessageErrors errors)
    {
        var result = await service.DeleteAsync(id, appointmentId);

        if (!result)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
