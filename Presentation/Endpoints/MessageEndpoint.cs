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

    public static IResult GetCreatedResult(int newId, int appointmentId) =>
        Results.Created($"appointment/{appointmentId}/chat/{newId}", new { Message = "Mensagem enviada com sucesso" });

    public static async Task<IResult> IsAllowedCheckAsync(
        int appointmentId,
        IMessageService service,
        IUserService userService)
    {
        int userId = await userService.GetMyUserIdAsync();
        var result = await service.CanSendMessageAsync(appointmentId, userId);
        return Results.Ok(result);
    }

    public static async Task<IResult> CreateMessageAsync(
        int appointmentId,
        MessageDtoCreate dto,
        IMessageService service,
        IUserService userService)
    {
        int senderId = await userService.GetMyUserIdAsync();
        var message = await service.CreateAsync(dto, appointmentId, senderId);
        return GetCreatedResult(message.Id, message.AppointmentId);
    }

    public static async Task<IResult> GetMessageAsync(
        int id,
        int appointmentId,
        IMessageService service)
    {
        var message = await service.GetByIdAsync(id, appointmentId);
        return Results.Ok(message);
    }

    public static async Task<IResult> GetAllMessagesAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int appointmentId,
        IMessageService service)
    {
        var messages = await service.GetAllAsync(page, pageSize, appointmentId);
        return Results.Ok(messages);
    }

    public static async Task<IResult> DeleteMessageAsync(
        int appointmentId,
        int id,
        IMessageService service,
        IUserService userService)
    {
        int senderId = await userService.GetMyUserIdAsync();
        await service.DeleteAsync(id, appointmentId, senderId);
        return Results.NoContent();
    }
}
