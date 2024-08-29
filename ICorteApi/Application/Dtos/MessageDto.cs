using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;

namespace ICorteApi.Application.Dtos;

public record MessageDtoRequest(
    string Content,
    DateTime SentAt
) : IDtoRequest<Message>;

public record MessageIsReadDtoRequest(
    int Id,
    bool IsRead
) : IDtoRequest<Message>;

public record MessageDtoResponse(
    int Id,
    string Content,
    DateTime SentAt,
    bool IsRead,
    int SenderId,
    string FirstName,
    string LastName
) : IDtoResponse<Message>;
