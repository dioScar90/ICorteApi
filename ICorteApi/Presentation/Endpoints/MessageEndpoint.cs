using ICorteApi.Application.Interfaces;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using ICorteApi.Presentation.Enums;
using ICorteApi.Domain.Interfaces;
using FluentValidation;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Presentation.Endpoints;

public static class MessageEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.Appointment + "{appointmentId}" + EndpointPrefixes.Message;
    private static readonly string ENDPOINT_NAME = EndpointNames.Message;

    public static IEndpointRouteBuilder MapMessageEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet(INDEX, GetAllMessages);
        group.MapGet("{id}", GetMessage);
        group.MapPost(INDEX, CreateMessage);
        group.MapPut("{id}", UpdateMessage);
        group.MapDelete("{id}", DeleteMessage);

        return app;
    }
    
    public static IResult GetCreatedResult(int newId, int appointmentId)
    {
        string uri = EndpointPrefixes.Appointment + "/" + appointmentId + "/" + EndpointPrefixes.Message + "/" + newId;
        object value = new { Message = "Mensagem enviada com sucesso" };
        return Results.Created(uri, value);
    }
    
    public static async Task<IResult> GetMessage(
        int id,
        IMessageService service,
        IMessageErrors errors)
    {
        var res = await service.GetByIdAsync(id);

        if (!res.IsSuccess)
            errors.ThrowNotFoundException();

        return Results.Ok(res.Value!.CreateDto());
    }

    public static async Task<IResult> GetAllMessages(
        int? page,
        int? pageSize,
        IMessageService service,
        IMessageErrors errors)
    {
        var res = await service.GetAllAsync(page, pageSize);

        if (!res.IsSuccess)
            errors.ThrowBadRequestException(res.Error.Description);
            
        var dtos = res.Values!
            .Select(c => c.CreateDto())
            .ToList();

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

        int senderId = (await userService.GetMeAsync()).Value!.Id;
        var res = await service.CreateAsync(dto, appointmentId, senderId);

        if (!res.IsSuccess)
            errors.ThrowCreateException();

        return GetCreatedResult(res.Value!.Id, appointmentId);
    }

    public static async Task<IResult> UpdateMessage(
        int id,
        MessageDtoRequest dto,
        IValidator<MessageDtoRequest> validator,
        IMessageService service,
        IMessageErrors errors)
    {
        dto.CheckAndThrowExceptionIfInvalid(validator, errors);
        
        var res = await service.UpdateAsync(dto, id);

        if (!res.IsSuccess)
            errors.ThrowUpdateException();

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteMessage(
        int id,
        IMessageService service,
        IMessageErrors errors)
    {
        var res = await service.DeleteAsync(id);

        if (!res.IsSuccess)
            errors.ThrowDeleteException();

        return Results.NoContent();
    }
}
