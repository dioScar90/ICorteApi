using ICorteApi.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Domain.Base;

public abstract class BasePrimaryKeyEntity<TKey> : IPrimaryKeyEntity<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}

public abstract class BasePrimaryKeyUserEntity<TKey> : IdentityUser<TKey>, IPrimaryKeyEntity<TKey>
    where TKey : IEquatable<TKey>
{
    // public TKey Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; } = false;
}

public abstract class CompositeKeyEntity<TKey1, TKey2> : ICompositeKeyEntity<TKey1, TKey2>
    // where TKey1 : IEquatable<TKey1>
    // where TKey2 : IEquatable<TKey2>
{
    public TKey1 Id1 { get; set; }
    public TKey2 Id2 { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = false;
}


