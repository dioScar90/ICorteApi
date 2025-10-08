using ICorteApi.Application.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ICorteApi.Presentation.Endpoints;

public static class MessageEndpoint
{
    private static string GetBaseEndpoint(MessageDtoResponse? message = null) => message is null
        ? "appointment/{appointmentId}/chat"
        : $"appointment/{message.AppointmentId}/chat/{message.Id}";
        
    public static IEndpointRouteBuilder MapMessageEndpoint(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup(GetBaseEndpoint()).WithTags("Chat");

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
    
    internal record LoggerActions
    {
        private string Entity;
        private ILogger Logger;

        private LoggerActions() { }

        internal static LoggerActions FactoryCreate(ILoggerFactory loggerFactory)
        {
            return new()
            {
                Entity = nameof(Message),
                Logger = loggerFactory.CreateLogger(nameof(MessageEndpoint))
            };
        }

        internal void CheckingAllowingStart(int appointmentId) =>
            Logger.LogInformation(
                "Received request to check if is allowed {Entity} with AppointmentId={AppointmentId}",
                Entity, appointmentId);

        internal void CreatingStart(MessageDtoRequest dto) =>
            Logger.LogInformation("Received request to create {Entity} {@Entity}", Entity, dto);

        internal void Created(int id) =>
            Logger.LogInformation("{Entity} successfully created with Id={Id}", Entity, id);

        internal void GettingStart(int id) =>
            Logger.LogInformation("Received request to get {Entity} with Id={Id}", Entity, id);

        internal void GettingAllStart(int appointmentId, int? page, int? pageSize) =>
            Logger.LogInformation(
                "Received request to get all {Entity} with AppointmentId={AppointmentId} Page={Page} PageSize={PageSize}",
                Entity, appointmentId, page, pageSize);
            
        internal void DeletingStart(int id) =>
            Logger.LogInformation("Received request to delete {Entity} Id={Id}", Entity, id);

        internal void Deleted(int id) =>
            Logger.LogInformation("{Entity} successfully deleted with Id={Id}", Entity, id);
    }
    
    public static async Task<Ok<bool>> IsAllowedCheckAsync(
        int appointmentId,
        MessageService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.CheckingAllowingStart(appointmentId);

        var result = await service.CanSendMessageAsync(appointmentId);
        return TypedResults.Ok(result);
    }

    public static async Task<Created<MessageDtoResponse>> CreateMessageAsync(
        int appointmentId,
        MessageDtoRequest dto,
        MessageService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        dto = dto with { AppointmentId = appointmentId };
        logger.CreatingStart(dto);

        var message = await service.CreateAsync(dto);

        logger.Created(message.Id);
        return TypedResults.Created(GetBaseEndpoint(message), message);
    }

    public static async Task<Ok<MessageDtoResponse>> GetMessageAsync(
        int id,
        int appointmentId,
        MessageService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingStart(id);

        var message = await service.GetByIdAsync(id, appointmentId);
        return TypedResults.Ok(message);
    }

    public static async Task<Ok<PaginationResponse<MessageDtoResponse>>> GetAllMessagesAsync(
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        int appointmentId,
        MessageService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.GettingAllStart(appointmentId, page, pageSize);

        var messages = await service.GetAllAsync(page, pageSize, appointmentId);
        return TypedResults.Ok(messages);
    }

    public static async Task<NoContent> DeleteMessageAsync(
        int appointmentId,
        int id,
        MessageService service,
        ILoggerFactory loggerFactory)
    {
        var logger = LoggerActions.FactoryCreate(loggerFactory);

        logger.DeletingStart(id);

        await service.DeleteAsync(id, appointmentId);

        logger.Deleted(id);
        return TypedResults.NoContent();
    }
}
