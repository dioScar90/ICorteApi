namespace ICorteApi.Application.Dtos;

public record ProfileDto(
    int Id,
    string FirstName,
    string LastName,
    string FullName,
    Gender Gender,
    string? ImageUrl
) : IDto<Profile>;
