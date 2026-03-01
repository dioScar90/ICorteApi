using System.ComponentModel.DataAnnotations;
using ICorteApi.Application.Services;
using ICorteApi.Application.Validators;

namespace ICorteApi.Application.Dtos;

public record UserDtoResponse(
    int Id,
    string Email,
    string PhoneNumber,
    string[] Roles,
    ProfileDtoResponse? Profile,
    BarberShopDtoResponse? BarberShop
) : IDtoResponse<User>;

public record UserDtoEmailUpdate(
    string Email
) : IDtoRequest<User>;

public record UserDtoPhoneNumberUpdate(
    string PhoneNumber
) : IDtoRequest<User>;

public record UserDtoRegisterRequest(
    [Required(ErrorMessage = "Email obrigatório")]
    [Email]
    string Email,
    
    [Required(ErrorMessage = "Senha obrigatória")]
    [Password]
    string Password,
    
    [property: Compare(nameof(UserDtoRegisterRequest.Password), ErrorMessage = "As senhas precisam ser iguais.")]
    string ConfirmPassword,

    ProfileDtoRequest? Profile = null,
    BarberShopDtoRequest? BarberShop = null
) : IDtoRequest<User>;

public record UserDtoLoginRequest(
    [Required(ErrorMessage = "Email obrigatório")]
    [Email]
    string Email,
    
    [Required(ErrorMessage = "Senha obrigatória")]
    string Password
) : IDtoRequest<User>;

public record UserDtoForgotPasswordRequest(
    [Required(ErrorMessage = "Email obrigatório")]
    [Email]
    string Email
) : IDtoRequest<User>;

public record UserDtoPasswordUpdateRequest(
    [Required(ErrorMessage = "Senha atual obrigatória")]
    string CurrentPassword,

    [Required(ErrorMessage = "Nova senha obrigatória")]
    [Password]
    string NewPassword,
    
    [property: Compare(nameof(UserDtoPasswordUpdateRequest.NewPassword), ErrorMessage = "As senhas precisam ser iguais.")]
    string ConfirmNewPassword
) : IDtoRequest<User>;

public record UserDtoResetPasswordRequest(
    [Required(ErrorMessage = "Email obrigatório")]
    [Email]
    string Email,
    
    string Token,
    
    [Required(ErrorMessage = "Nova senha obrigatória")]
    [Password]
    string NewPassword,
    
    [property: Compare(nameof(UserDtoPasswordUpdateRequest.NewPassword), ErrorMessage = "As senhas precisam ser iguais.")]
    string ConfirmNewPassword
) : IDtoRequest<User>;

public record UserDtoUpdateProfileRequest(
    [Required(ErrorMessage = "Nome obrigatório")]
    [MinLength(3, ErrorMessage = "Nome precisa ter pelo menos 3 caracteres")]
    string FirstName,

    [Required(ErrorMessage = "Sobrenome obrigatório")]
    [MinLength(3, ErrorMessage = "Sobrenome precisa ter pelo menos 3 caracteres")]
    string LastName
) : IDtoRequest<User>;

public record UserDtoConfirmEmailRequest(
    [Required(ErrorMessage = "Email obrigatório")]
    [Email]
    string Email,

    string Token
) : IDtoRequest<User>;

public record FoundUserByAdmin(
    int Id,
    
    [Required(ErrorMessage = "Nome obrigatório")]
    [MinLength(3, ErrorMessage = "Nome precisa ter pelo menos 3 caracteres")]
    string FirstName,
    
    [Required(ErrorMessage = "Sobrenome obrigatório")]
    [MinLength(3, ErrorMessage = "Sobrenome precisa ter pelo menos 3 caracteres")]
    string LastName,

    [Required(ErrorMessage = "Email obrigatório")]
    [Email]
    string Email,

    [Required(ErrorMessage = "Número de telefone obrigatório")]
    [PhoneNumber]
    string PhoneNumber,
    
    bool IsBarberShop
) : IDtoRequest<User>;
