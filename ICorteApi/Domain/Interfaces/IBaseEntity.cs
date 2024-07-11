namespace ICorteApi.Domain.Interfaces;

public interface IBaseEntity : IBaseCrudEntity
{
    int Id { get; set; }
}
