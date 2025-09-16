namespace ICorteApi.Application.Dtos;

public record MessageDto(
    int Id,
    int AppointmentId,
    int SenderId,
    string Content,
    DateTime SentAt,
    bool IsRead,
    string FirstName,
    string LastName
) : IDto<Message>;

public record ChatWithMessagesDto(
    int AppointmentId,
    bool IsMe,
    string Content,
    DateTime SentAt,
    string FirstName,
    bool IsRead
) : IDto<Message>;
