using ICorteApi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class ChatEndpoint
{
    public static IEndpointRouteBuilder MapMessageEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("appointment/{appointmentId}/chat").WithTags("Chat");

        group.MapGet("check", IsAllowedCheckAsync)
            .WithSummary("Is Allowed Check")
            .WithDescription("Check if the user has permission to send messages in an specific appointment.")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapPost("", CreateMessageAsync)
            .WithSummary("Create Message")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("{id}", GetMessageAsync)
            .WithSummary("Get Message")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapGet("", GetAllMessagesAsync)
            .WithSummary("Get All Messages")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));

        group.MapDelete("{id}", DeleteMessageAsync)
            .WithSummary("Delete Message")
            .RequireAuthorization(nameof(PolicyUserRole.ClientOrHigh));
            
        return app;
    }
    
    private static IResult GetCreatedResult(MessageDtoResponse dto) =>
        Results.Created($"appointment/{dto.AppointmentId}/chat/{dto.Id}", new { Message = "Mensagem enviada com sucesso", Item = dto });

    public static async Task<IResult> IsAllowedCheckAsync(
        int appointmentId,
        MessageService service)
    {
        var result = await service.CanSendMessageAsync(appointmentId);
        return Results.Ok(result);
    }

    public static async Task<IResult> CreateMessageAsync(
        int appointmentId,
        MessageDtoRequest dto,
        MessageService service)
    {
        dto = dto with { AppointmentId = appointmentId };
        var message = await service.CreateAsync(dto);
        return GetCreatedResult(message);
    }

    public static async Task<IResult> GetMessageAsync(
        int id,
        int appointmentId,
        MessageService service)
    {
        var message = await service.GetByIdAsync(id, appointmentId);
        return Results.Ok(message);
    }

    public static async Task<IResult> GetAllMessagesAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int appointmentId,
        MessageService service)
    {
        var messages = await service.GetAllAsync(page, pageSize, appointmentId);
        return Results.Ok(messages);
    }

    public static async Task<IResult> DeleteMessageAsync(
        int appointmentId,
        int id,
        MessageService service)
    {
        await service.DeleteAsync(id, appointmentId);
        return Results.NoContent();
    }
}
