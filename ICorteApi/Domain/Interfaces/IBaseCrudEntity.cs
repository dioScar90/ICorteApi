namespace ICorteApi.Domain.Interfaces;

public interface IBaseCrudEntity : IBaseTableEntity
{
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    bool? IsDeleted { get; set; }
}
