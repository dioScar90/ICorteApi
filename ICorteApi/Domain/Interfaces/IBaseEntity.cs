using ICorteApi.Application.Interfaces;

namespace ICorteApi.Domain.Interfaces;

public interface IBaseEntity : IBaseCrudEntity
{
    int Id { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    bool IsDeleted { get; set; }
}
