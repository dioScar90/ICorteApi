using System.ComponentModel.DataAnnotations;
using ICorteApi.Application.Services;

namespace ICorteApi.Application.Dtos;

public record ProfileDtoResponse(
    int Id,
    string FirstName,
    string LastName,
    string FullName,
    Gender Gender,
    string? ImageUrl
) : IDtoResponse<Profile>;

public record ProfileDtoRequest(
    int Id,

    [Required(ErrorMessage = "Nome obrigatório")]
    [MinLength(3, ErrorMessage = "Nome precisa ter pelo menos 3 caracteres")]
    string FirstName,

    [Required(ErrorMessage = "Sobrenome obrigatório")]
    [MinLength(3, ErrorMessage = "Sobrenome precisa ter pelo menos 3 caracteres")]
    string LastName,

    [Required(ErrorMessage = "Gênero não pode estar vazio")]
    [EnumDataType(typeof(Gender), ErrorMessage = "Gênero inválido")]
    Gender Gender
) : IDtoRequest<Profile>;
