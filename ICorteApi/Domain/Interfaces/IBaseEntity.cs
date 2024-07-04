namespace ICorteApi.Domain.Interfaces;

public interface IBaseEntity : IBaseSoftCrudEntity
{
    int Id { get; set; }
}
