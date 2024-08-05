using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;

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
        _entity = entityName;
        _isFemale = isFemale;
        _the = _isFemale ? 'a' : 'o';
    }

    public Error CreateError() =>
        new("Create Error", $"Não foi possível criar {_the} {_entity}");
    
    public Error UpdateError() =>
        new("Update Error", $"Não foi possível atualizar {_the} {_entity}");
    
    public Error DeleteError() =>
        new("Remove Error", $"Não foi possível excluir {_the} {_entity}");
    
    public Error NotFoundError() =>
        new("Search Error", NotFoundEntityMessage());
    
    private static string UcWord(string entity) =>
        string.Join(' ', entity
            .Split(' ')
            .Where(a => !string.IsNullOrWhiteSpace(a) && a.Length > 1)
            .Select(a => a.Length > 2 ? char.ToUpper(a[0]) + a[1..].ToLower() : a.ToLower())
        );

    private string NotFoundEntityMessage()
    {
        string entidade = UcWord(_entity);
        string encontrada = _isFemale ? "encontrada" : "encontrado";
        return $"{entidade} não {encontrada}";
    }

    private (string, bool) GetEntityProps(Type entityType) => entityType.Name switch
        {
            nameof(Address)             => ("endereço", false),
            nameof(Appointment)         => ("agendamento", false),
            nameof(BarberShop)          => ("barbearia", true),
            nameof(Conversation)        => ("conversa", true),
            nameof(Message)             => ("mensagem", true),
            nameof(Person)              => ("pessoa", true),
            nameof(RecurringSchedule)   => ("horário de funcionamento", false),
            nameof(Report)              => ("avaliação", true),
            nameof(Service)             => ("serviço", false),
            nameof(SpecialSchedule)     => ("horário especial", false),
            nameof(User)                => ("usuário", false),
            _ => ("error", false)
        };
}
