using ICorteApi.Enums;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Entities;

public class Role : IdentityRole<int>
{
    public UserRole Name { get; set; }
}
