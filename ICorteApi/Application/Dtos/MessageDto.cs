using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record MessageDtoRequest(
    string Content,
    DateTime Timestamp
) : IDtoRequest;

public record MessageDtoResponse(
    int Id,
    string Content,
    DateTime Timestamp,
    PersonDtoResponse Sender
) : IDtoResponse;
