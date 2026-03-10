

using ICorteApi.Application.Services;

namespace ICorteApi.Domain.Utils;

public record LoggerActions<TBaseEntity>
    where TBaseEntity : class, IBaseTableEntity
{
    private string Entity { get; init; }
    private ILogger Logger { get; init; }
    
    private LoggerActions(string entity, ILoggerFactory loggerFactory)
    {
        Entity = entity;
        Logger = loggerFactory.CreateLogger($"{Entity}Endpoint");
    }
    
    public static LoggerActions<T> FactoryCreate<T>(ILoggerFactory loggerFactory)
        where T : class, IBaseTableEntity
    {
        return new(typeof(T).GetType().Name, loggerFactory);
    }

    public void CreatingStart(IDtoRequest<TBaseEntity> dto) =>
        Logger.LogInformation("Received request to create {Entity} {@Entity}", Entity, dto);

    public void Created(params object[] ids) =>
        Logger.LogInformation("{Entity} successfully created with Id={@Id}", Entity, ids);

    public void GettingStart(params object[] ids) =>
        Logger.LogInformation("Received request to get {Entity} with Id={@Id}", Entity, ids);

    public void GettingAllStart(int? page, int? pageSize) =>
        Logger.LogInformation("Received request to get all {Entity} with Page={Page} PageSize={PageSize}", Entity, page, pageSize);

    public void UpdatingStart(IDtoRequest<TBaseEntity> dto, params object[] ids) =>
        Logger.LogInformation("Received request to update {Entity} with Id={@Id} {@Entity}", Entity, ids, dto);

    public void Updated(params object[] ids) =>
        Logger.LogInformation("{Entity} successfully updated with Id={@Id}", Entity, ids);
        
    public void DeletingStart(params object[] ids) =>
        Logger.LogInformation("Received request to delete {Entity} Id={@Id}", Entity, ids);

    public void Deleted(params object[] ids) =>
        Logger.LogInformation("{Entity} successfully deleted with Id={@Id}", Entity, ids);
}