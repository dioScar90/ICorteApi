using Microsoft.AspNetCore.Http.HttpResults;

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
        _entity = entityName.UcWords();
        _isFemale = isFemale;
        _the = _isFemale ? 'a' : 'o';
    }
    
    private (string, bool) GetEntityProps(Type entityType) => entityType.Name switch
        {
            nameof(Address)             => ("endereço", false),
            nameof(Appointment)         => ("agendamento", false),
            nameof(BarberShop)          => ("barbearia", true),
            nameof(Message)             => ("mensagem", true),
            nameof(Profile)             => ("perfil", true),
            nameof(RecurringSchedule)   => ("horário de funcionamento", false),
            nameof(Report)              => ("avaliação", true),
            nameof(Service)             => ("serviço", false),
            nameof(SpecialSchedule)     => ("horário especial", false),
            nameof(User)                => ("usuário", false),
            _ => ("error", false)
        };
        
    public BadRequest<Error> BadRequest(string? message = null, params Error[] errors)
    {
        message ??= $"Não foi possível concluir a operação {_the} {_entity}";
        return Error.BadRequest(message, errors);
    }
    
    public BadRequest<Error> Create(params Error[] errors)
    {
        string message = $"Não foi possível criar {_the} {_entity}";
        return BadRequest(message, errors);
    }

    public BadRequest<Error> Update(params Error[] errors)
    {
        string message = $"Não foi possível atualizar {_the} {_entity}";
        return BadRequest(message, errors);
    }
    
    public BadRequest<Error> Delete(params Error[] errors)
    {
        string message = $"Não foi possível excluir {_the} {_entity}";
        return BadRequest(message);
    }

    public NotFound<Error> NotFound()
    {
        string encontrada = _isFemale ? "encontrada" : "encontrado";
        string message = $"{_entity} não {encontrada}";

        return Error.NotFound(message);
    }
}
