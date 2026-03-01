using System.ComponentModel.DataAnnotations;
using ICorteApi.Application.Services;

namespace ICorteApi.Application.Dtos;

public record AddressDtoResponse(
    int Id,
    int BarberShopId,
    string Street,
    string Number,
    string? Complement,
    string Neighborhood,
    string City,
    State State,
    string PostalCode,
    string Country
) : IDtoResponse<Address>;

public record AddressDtoRequest(
    [Required]
    int BarberShopId,
    
    [Required(ErrorMessage = "Logradouro obrigatório")]
    [MinLength(3, ErrorMessage = "Logradouro precisa ter pelo menos 3 caracteres")]
    string Street,
    
    [Required(ErrorMessage = "Número obrigatório")]
    [property: Compare(nameof(AddressDtoRequest.Street), ErrorMessage = "Número não pode ser igual ao logradouro")]
    string Number,

    [MinLength(3, ErrorMessage = "Complemento precisa ter pelo menos 3 caracteres")]
    string? Complement,

    [Required(ErrorMessage = "Bairro obrigatório")]
    [MinLength(3, ErrorMessage = "Bairro precisa ter pelo menos 3 caracteres")]
    string Neighborhood,

    [Required(ErrorMessage = "Cidade obrigatória")]
    [MinLength(3, ErrorMessage = "Cidade precisa ter pelo menos 3 caracteres")]
    string City,
    
    [Required(ErrorMessage = "Estado obrigatório")]
    [EnumDataType(typeof(State), ErrorMessage = "Estado inválido")]
    State State,
    
    [Required(ErrorMessage = "CEP obrigatório")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "CEP precisa ter 8 dígitos")]
    string PostalCode,
    
    [Required(ErrorMessage = "País obrigatório")]
    [MinLength(3, ErrorMessage = "País precisa ter pelo menos 3 caracteres")]
    string Country
) : IDtoRequest<Address>;
