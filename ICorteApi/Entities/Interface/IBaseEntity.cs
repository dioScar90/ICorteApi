using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICorteApi.Entities;

public interface IBaseEntity {
    int Id { get; set; }
    DateTime CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    bool IsDeleted { get; set; }
}
