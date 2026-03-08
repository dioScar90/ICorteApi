using System.ComponentModel.DataAnnotations;
using ICorteApi.Application.Services;
using ICorteApi.Application.Validators;

namespace ICorteApi.Application.Dtos;

public record ResetPasswordDto(
    [Required(ErrorMessage = "Email obrigatório")]
    [Email]
    string Email
) : IDtoRequest;
