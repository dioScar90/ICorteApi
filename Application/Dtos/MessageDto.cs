using ICorteApi.Application.Services;

namespace ICorteApi.Application.Dtos;

public record MessageDtoResponse(
    int Id,
    int AppointmentId,
    int SenderId,
    string Content,
    DateTime SentAt,
    bool IsRead,
    string FirstName,
    string LastName
) : IDtoResponse<Message>;

public record MessageDtoRequest(
    int Id,
    int AppointmentId,
    int SenderId,
    string Content,
    DateTime SentAt,
    bool IsRead,
    string FirstName,
    string LastName
) : IDtoRequest<Message>;

public record ChatWithMessagesDto(
    int AppointmentId,
    bool IsMe,
    string Content,
    DateTime SentAt,
    string FirstName,
    bool IsRead
) : IDto<Message>;
