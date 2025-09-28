using ICorteApi.Application.Services;

namespace ICorteApi.Application.Dtos;

public record ProfileDtoResponse(
    int Id,
    string FirstName,
    string LastName,
    string FullName,
    Gender Gender,
    string? ImageUrl
) : IDtoResponse<Profile>;

public record ProfileDtoRequest(
    int Id,
    string FirstName,
    string LastName,
    string FullName,
    Gender Gender,
    string? ImageUrl
) : IDtoRequest<Profile>;
