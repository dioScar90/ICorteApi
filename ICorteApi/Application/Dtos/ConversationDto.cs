using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Dtos;

public record ConversationDtoRequest(
    DateTime? LastMessageAt
) : IDtoRequest<Conversation>;

public record ConversationDtoResponse(
    int Id,
    DateTime? LastMessageAt
) : IDtoResponse<Conversation>;
