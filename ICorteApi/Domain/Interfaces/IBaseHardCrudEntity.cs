namespace ICorteApi.Domain.Interfaces;

public interface IBaseHardCrudEntity : IBaseTableEntity
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    bool IsActive { get; set; }
}
