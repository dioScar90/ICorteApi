using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Presentation.Exceptions;

namespace ICorteApi.Domain.Errors;

public abstract class BaseErrors<TEntity> : IBaseErrors<TEntity>
    where TEntity : class, IBaseTableEntity
{
    protected readonly string _entity;
    protected readonly bool _isFemale;
    protected readonly char _the;

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
            nameof(Message)             => ("mensagem", true),
            nameof(Payment)             => ("pagamento", true),
            nameof(Profile)             => ("perfil", true),
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
        
    public void ThrowCreateException(params Error[]? errors)
    {
        string message = $"Não foi possível criar {_the} {_entity}";
        throw new BadRequestException(message, errors);
    }

    public void ThrowUpdateException(params Error[]? errors)
    {
        string message = $"Não foi possível atualizar {_the} {_entity}";
        throw new BadRequestException(message, errors);
    }
    
    public void ThrowDeleteException(params Error[]? errors)
    {
        string message = $"Não foi possível excluir {_the} {_entity}";
        throw new BadRequestException(message, errors);
    }
    
    public void ThrowBadRequestException(params Error[]? errors)
    {
        string message = $"Não foi possível concluir a operação {_the} {_entity}";
        throw new BadRequestException(message, errors);
    }

    public void ThrowNotFoundException(params Error[]? errors)
    {
        string encontrada = _isFemale ? "encontrada" : "encontrado";
        string message = $"{_entity} não {encontrada}";
        throw new NotFoundException(message, errors);
    }

    public void ThrowValidationException(params Error[]? errors)
    {
        string da = _isFemale ? "da" : "do";
        string message = $"Um ou mais itens {da} {_entity} inválidos";
        throw new UnprocessableEntity(message, errors);
    }
}
