using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Entities;

public abstract class BaseUser : IdentityUser<int>, IBaseEntity, IBaseTableEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool? IsDeleted { get; set; }
}
