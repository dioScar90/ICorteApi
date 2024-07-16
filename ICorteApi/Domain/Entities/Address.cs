using System.ComponentModel.DataAnnotations;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Domain.Entities;

public class Address : BaseEntity
{
    [Required(ErrorMessage = "Endereço é obrigatório")]
    [StringLength(100, ErrorMessage = "Endereço não pode ser maior que 100 caracteres")]
    public required string Street { get; set; }

    [Required(ErrorMessage = "Número é obrigatório")]
    public required string Number { get; set; }

    [StringLength(100, MinimumLength = 3, ErrorMessage = "Complemento precisa ter pelo menos 3 caracteres")]
    public string? Complement { get; set; }

    [Required(ErrorMessage = "Bairro é obrigatório")]
    public required string Neighborhood { get; set; }

    [Required(ErrorMessage = "Cidade é obrigatória")]
    public required string City { get; set; }

    [Required(ErrorMessage = "Estado é obrigatório")]
    public required State State { get; set; }

    [Required(ErrorMessage = "CEP é obrigatório")]
    [StringLength(8, MinimumLength = 8, ErrorMessage = "CEP precisa ter 8 dígitos")]
    public required string PostalCode { get; set; }

    [Required(ErrorMessage = "País é obrigatório")]
    public required string Country { get; set; }
    
    public int BarberShopId { get; set; }
    public BarberShop? BarberShop { get; set; }
}
