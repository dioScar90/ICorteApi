using System.ComponentModel.DataAnnotations;
using BarberAppApi.Enums;
using BarberAppApi.Validators;

namespace BarberAppApi.Dtos;

public record UserDto(
    string Username,
    string Email,
    string Phone,
    Role Role,
    string Password
);
