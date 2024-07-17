using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record MessageDtoRequest(
    string Content
) : IDtoRequest;

public record MessageDtoResponse(
    int Id,
    string Content,
    DateTime Timestamp,
    PersonDtoResponse Sender
) : IDtoResponse;
