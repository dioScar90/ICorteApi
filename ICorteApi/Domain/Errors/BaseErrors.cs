using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Exceptions;

namespace ICorteApi.Domain.Errors;

public abstract class BaseErrors<TEntity> : IBaseErrors<TEntity>
    where TEntity : class, IBaseTableEntity
{
    private readonly string _entity;
    private readonly bool _isFemale;
    private readonly char _the;

    protected BaseErrors()
    {
        (string entityName, bool isFemale) = GetEntityProps(typeof(TEntity));
        _entity = UcWord(entityName);
        _isFemale = isFemale;
        _the = _isFemale ? 'a' : 'o';
    }
    
    private (string, bool) GetEntityProps(Type entityType) => entityType.Name switch
        {
            nameof(Address)             => ("endereço", false),
            nameof(Appointment)         => ("agendamento", false),
            nameof(BarberShop)          => ("barbearia", true),
            // nameof(Conversation)        => ("conversa", true),
            nameof(Message)             => ("mensagem", true),
            // nameof(Person)              => ("pessoa", true),
            nameof(RecurringSchedule)   => ("horário de funcionamento", false),
            nameof(Report)              => ("avaliação", true),
            nameof(Service)             => ("serviço", false),
            nameof(SpecialSchedule)     => ("horário especial", false),
            nameof(User)                => ("usuário", false),
            _ => ("error", false)
        };
        
    private static string UcWord(string entity) =>
        string.Join(' ', entity
            .Split(' ')
            .Where(a => !string.IsNullOrWhiteSpace(a) && a.Length > 1)
            .Select(a => a.Length > 2 ? char.ToUpper(a[0]) + a[1..].ToLower() : a.ToLower())
        );

    private string CreateErrorMessage() => $"Não foi possível criar {_the} {_entity}";
    private string UpdateErrorMessage() => $"Não foi possível atualizar {_the} {_entity}";
    private string DeleteErrorMessage() => $"Não foi possível excluir {_the} {_entity}";

    private string NotFoundEntityMessage()
    {
        string encontrada = _isFemale ? "encontrada" : "encontrado";
        return $"{_entity} não {encontrada}";
    }

    private string ValidationErrorMessage()
    {
        string da = _isFemale ? "da" : "do";
        return $"Erro ao tentar validar um ou mais itens {da} {_entity}";
    }
    
    public void ThrowCreateException()
    {
        throw new BadRequestException(CreateErrorMessage());
    }

    public void ThrowUpdateException()
    {
        throw new BadRequestException(UpdateErrorMessage());
    }
    
    public void ThrowDeleteException()
    {
        throw new BadRequestException(DeleteErrorMessage());
    }

    public void ThrowNotFoundException()
    {
        throw new NotFoundException(NotFoundEntityMessage());
    }

    public void ThrowValidationException(Error[] errors)
    {
        throw new UnprocessableEntity(ValidationErrorMessage(), errors);
    }

    public void ThrowValidationException(IDictionary<string, string[]> errors)
    {
        throw new UnprocessableEntity(ValidationErrorMessage(), errors);
    }

    public void ThrowBadRequestException(string? message = null)
    {
        string finalMessage = "Não foi possível concluir a operação";

        if (!string.IsNullOrWhiteSpace(message))
            finalMessage += ": " + message;

        throw new BadRequestException(finalMessage);
    }
}
