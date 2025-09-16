using FluentValidation;

namespace ICorteApi.Application.Validators;

public sealed class ProfileDtoValidator : AbstractValidator<ProfileDto>
{
    public ProfileDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Nome obrigatório")
            .MinimumLength(3).WithMessage("Nome precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Sobrenome obrigatório")
            .MinimumLength(3).WithMessage("Sobrenome precisa ter pelo menos 3 caracteres");

        RuleFor(x => x.Gender)
            .NotNull().WithMessage("Gênero não pode estar vazio")
            .IsInEnum().WithMessage("Gênero inválido");

        RuleFor(x => x.PhoneNumber).ApplyPhoneNumberValidation();
    }
}
