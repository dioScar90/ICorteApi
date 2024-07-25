namespace ICorteApi.Domain.Interfaces;

public interface IPrimaryKeyEntity<TKey>
{
    TKey Key { get; }
}
