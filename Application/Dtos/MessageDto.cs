using System.ComponentModel.DataAnnotations;
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

public record ChatWithMessagesDtoResponse(
    int AppointmentId,
    bool IsMe,
    string Content,
    DateTime SentAt,
    string FirstName,
    bool IsRead
) : IDtoResponse<Message>;

public record MessageDtoRequest(
    int Id,
    int AppointmentId,
    int SenderId,

    [Required(ErrorMessage = "Mensagem não pode estar vazia")]
    [MaxLength(255, ErrorMessage = "Mensagem não pode ser maior que 255 caracteres")]
    string Content,

    DateTime SentAt,
    bool IsRead,
    string FirstName,
    string LastName
) : IDtoRequest<Message>;

public record MessageDtoIsReadUpdateRequest(
    int Id,
    bool IsRead
) : IDtoRequest<Message>;
