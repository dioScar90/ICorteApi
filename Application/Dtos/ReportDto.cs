using System.ComponentModel.DataAnnotations;
using ICorteApi.Application.Services;

namespace ICorteApi.Application.Dtos;

public record ReportDtoResponse(
    int Id,
    int BarberShopId,
    string? Title,
    string? Content,
    int Rating
) : IDtoResponse<Report>;

public record ReportDtoRequest(
    int Id,
    int BarberShopId,

    [MinLength(3, ErrorMessage = "Título precisa ter pelo menos 3 caracteres")]
    string? Title,

    [MinLength(3, ErrorMessage = "Comentário precisa ter pelo menos 3 caracteres")]
    string? Content,
    
    [Required(ErrorMessage = "Nota não pode estar vazia")]
    [Range(1, 5, ErrorMessage = "Nota precisa estar entre 1 e 5")]
    int Rating
) : IDtoRequest<Report>;
