using System.ComponentModel.DataAnnotations;
using ICorteApi.Entities;
using ICorteApi.Enums;
using ICorteApi.Validators;

namespace ICorteApi.Dtos;

public record BarberShopDtoRequest(
    string Name,
    string? Description,
    string PhoneNumber,
    string ComercialNumber,
    string ComercialEmail,
    TimeSpan OpeningHours,
    TimeSpan ClosingHours,
    string DaysOpen,
    double Rating,
    AddressDtoRequest? Address,
    PersonDtoRequest[]? Barbers
    // Schedule[]? Schedules,
    // Service[]? Services
) : IDtoRequest;

public record BarberShopDtoResponse(
    string Name,
    string? Description,
    string PhoneNumber,
    string ComercialNumber,
    string ComercialEmail,
    TimeSpan OpeningHours,
    TimeSpan ClosingHours,
    string DaysOpen,
    double Rating,
    
    AddressDtoResponse? Address,
    PersonDtoResponse[]? Barbers
    // Schedule[]? Schedules,
    // Service[]? Services
) : IDtoResponse;
