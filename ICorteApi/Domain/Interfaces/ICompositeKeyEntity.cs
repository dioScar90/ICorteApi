namespace ICorteApi.Domain.Interfaces;

public interface ICompositeKeyEntity<TKey1, TKey2>
{
    TKey1 Key1 { get; }
    TKey2 Key2 { get; }
}
