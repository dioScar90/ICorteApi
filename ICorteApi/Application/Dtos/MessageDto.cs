using ICorteApi.Application.Interfaces;

namespace ICorteApi.Application.Dtos;

public record MessageDtoRequest(
    string Content,
    DateTime SentAt,
    bool IsRead,
    PersonDtoRequest Sender
) : IDtoRequest;

public record MessageDtoResponse(
    int Id,
    string Content,
    DateTime SentAt,
    bool IsRead,
    PersonDtoResponse Sender
) : IDtoResponse;
