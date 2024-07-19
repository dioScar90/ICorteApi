using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record ConversationDtoRequest(
    DateTime? LastMessageAt
) : IDtoRequest;

public record ConversationDtoResponse(
    int Id,
    DateTime? LastMessageAt
) : IDtoResponse;
