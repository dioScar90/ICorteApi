using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Interfaces;

public interface IHasNoForeignKeyService<TEntity> : IService<TEntity> where TEntity : class, IBaseTableEntity
{
    Task<ISingleResponse<TEntity>> CreateAsync(IDtoRequest<TEntity> dto);
}

public interface IHasOneForeignKeyService<TEntity, TForeignKey1> : IService<TEntity> where TEntity : class, IBaseTableEntity
{
    Task<ISingleResponse<TEntity>> CreateAsync(IDtoRequest<TEntity> dto, TForeignKey1 foreignKey1);
}

public interface IHasTwoForeignKeyService<TEntity, TForeignKey1, TForeignKey2> : IService<TEntity> where TEntity : class, IBaseTableEntity
{
    Task<ISingleResponse<TEntity>> CreateAsync(IDtoRequest<TEntity> dto, TForeignKey1 foreignKey1, TForeignKey2 foreignKey2);
}
