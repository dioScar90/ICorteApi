using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Application.Dtos;
using ICorteApi.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc;
using ICorteApi.Presentation.Enums;

namespace ICorteApi.Presentation.Endpoints;

public static class ConversationEndpoint
{
    private static readonly string INDEX = "";
    private static readonly string ENDPOINT_PREFIX = EndpointPrefixes.Conversation;
    private static readonly string ENDPOINT_NAME = EndpointNames.Conversation;

    public static void Map(WebApplication app)
    {
        var group = app.MapGroup(ENDPOINT_PREFIX)
            .WithTags(ENDPOINT_NAME)
            .RequireAuthorization();

        group.MapGet(INDEX, GetAllConversations);
        group.MapGet("{id}", GetConversation);
        group.MapPost(INDEX, CreateConversation);
        group.MapPut("{id}", UpdateConversation);
        group.MapDelete("{id}", DeleteConversation);
    }

    public static async Task<IResult> GetConversation(int id, IConversationService conversationService)
    {
        var response = await conversationService.GetByIdAsync(id);

        if (!response.IsSuccess)
            return Results.NotFound();

        var conversationDto = response.Value!.CreateDto();
        return Results.Ok(conversationDto);
    }

    public static async Task<IResult> GetAllConversations(
        int page,
        int pageSize,
        IConversationService conversationService)
    {
        var response = await conversationService.GetAllAsync(page, pageSize);

        if (!response.IsSuccess)
            return Results.BadRequest(response.Error);
            
        var dtos = response.Values!
            .Select(c => c.CreateDto())
            .ToList();

        return Results.Ok(dtos);
    }

    public static async Task<IResult> CreateConversation(
        ConversationDtoRequest dto,
        IConversationService conversationService)
    {
        var response = await conversationService.CreateAsync(dto);

        if (!response.IsSuccess)
            Results.BadRequest(response.Error);

        string uri = $"/{ENDPOINT_PREFIX}/{response.Value!.Id}";
        return Results.Created(uri, new { Message = "Conversa criada com sucesso" });
    }

    public static async Task<IResult> UpdateConversation(
        int id,
        ConversationDtoRequest dto,
        IConversationService conversationService)
    {
        var response = await conversationService.UpdateAsync(id, dto);

        if (!response.IsSuccess)
            return Results.NotFound(response.Error);

        return Results.NoContent();
    }

    public static async Task<IResult> DeleteConversation(int id, IConversationService conversationService)
    {
        var response = await conversationService.DeleteAsync(id);

        if (!response.IsSuccess)
            return Results.BadRequest(response.Error);

        return Results.NoContent();
    }
}
