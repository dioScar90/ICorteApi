using FluentValidation;

namespace ICorteApi.Application.Validators;

public class ProfileDtoUpdateValidator : AbstractValidator<ProfileDtoUpdate>
{
    public ProfileDtoUpdateValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Primeiro nome é obrigatório")
            .MinimumLength(3).WithMessage("Primeiro nome precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Sobrenome é obrigatório")
            .MinimumLength(3).WithMessage("Sobrenome precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.Gender)
            .NotNull().WithMessage("Gênero não pode estar vazia")
            .IsInEnum().WithMessage("Gênero inválido");

        // RuleFor(x => x.PhoneNumber)
        //     .NotEmpty().WithMessage("Número de telefone é obrigatório")
        //     .Length(11).WithMessage("Número de telefone precisa ter pelo menos 3 caracteres")
        //     .Must(IsValidPhoneNumber).WithMessage("Número de telefone precisa estar no formato (xx) 9xxxx-xxxx");
    }

    // private bool IsValidPhoneNumber(string phoneNumber) => Regex.IsMatch(phoneNumber, @"^\d{2}9\d{8}$");
}
