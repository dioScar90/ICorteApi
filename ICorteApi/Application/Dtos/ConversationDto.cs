using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record ConversationDtoRequest(
    int BarberId,
    int ClientId
) : IDtoRequest;

public record ConversationDtoResponse(
    int BarberId,
    int ClientId
) : IDtoResponse;
