using System.ComponentModel.DataAnnotations;
using ICorteApi.Enums;
using ICorteApi.Validators;
using Microsoft.AspNetCore.Identity;

namespace ICorteApi.Entities;

public class User : BaseUser
{
    public Person? Person { get; set; }
}
