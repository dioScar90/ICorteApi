namespace ICorteApi.Application.Dtos;

public record ProfileDtoCreate(
    string FirstName,
    string LastName,
    Gender Gender,
    string PhoneNumber
) : IDtoRequest<Profile>;

public record ProfileDtoUpdate(
    string FirstName,
    string LastName,
    Gender Gender
) : IDtoRequest<Profile>;

public record ProfileDtoResponse(
    int Id,
    string FirstName,
    string LastName,
    string FullName,
    Gender Gender,
    string? ImageUrl
) : IDtoResponse<Profile>;
