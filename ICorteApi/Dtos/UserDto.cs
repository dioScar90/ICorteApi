using System.ComponentModel.DataAnnotations;
using ICorteApi.Enums;
using ICorteApi.Validators;

namespace ICorteApi.Dtos;

public record UserDto(
    string Username,
    string Email,
    string Phone,
    Role Role,
    string Password
);
