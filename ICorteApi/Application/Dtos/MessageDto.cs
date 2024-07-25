using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Dtos;

public record MessageDtoRequest(
    string Content,
    DateTime SentAt,
    bool IsRead,
    PersonDtoRequest Sender
) : IDtoRequest<Message>;

public record MessageDtoResponse(
    int Id,
    string Content,
    DateTime SentAt,
    bool IsRead,
    PersonDtoResponse Sender
) : IDtoResponse<Message>;
