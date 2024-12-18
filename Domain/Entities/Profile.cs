using ICorteApi.Domain.Base;

namespace ICorteApi.Domain.Entities;

public sealed class Profile : BaseEntity<Profile>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public Gender Gender { get; private set; }
    public string? ImageUrl { get; private set; }

    private readonly string _phoneNumber;

    public User User { get; init; }

    private Profile() { }

    public Profile(ProfileDtoCreate dto, int? userId = null)
    {
        Id = userId ?? default;
        FirstName = dto.FirstName;
        LastName = dto.LastName;
        Gender = dto.Gender;
        // ImageUrl = userId is int idToPlaceholder ? GetImageUrlPlaceholder(idToPlaceholder, dto.Gender) : default;
        ImageUrl = default;

        _phoneNumber = dto.PhoneNumber;
    }

    public static string GetProfileBaseImageUrlPlaceholder() => "https://randomuser.me/api/portraits";
    
    private static string GetImageUrlPlaceholder(int id, Gender gender)
    {
        string baseUrl = GetProfileBaseImageUrlPlaceholder();
        string sex = gender is Gender.Male ? "men" : "women";
        id = 99 - id;
        return $"{baseUrl}/{sex}/{id}.jpg";
    }

    private void UpdateImageUrlIfFirstTime() => ImageUrl ??= GetImageUrlPlaceholder(Id, Gender);

    public string GetPhoneNumberToUserEntity() => _phoneNumber;

    private void UpdateByUserDto(ProfileDtoUpdate dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        FirstName = dto.FirstName;
        LastName = dto.LastName;
        Gender = dto.Gender;

        // UpdateImageUrlIfFirstTime();

        UpdatedAt = utcNow;
    }
    
    public override void UpdateEntityByDto(IDtoRequest<Profile> requestDto, DateTime? utcNow = null)
    {
        switch (requestDto)
        {
            case ProfileDtoUpdate dto:
                UpdateByUserDto(dto, utcNow);
                break;
            default:
                throw new ArgumentException("Tipo de DTO inválido", nameof(requestDto));
        }
    }

    public override ProfileDtoResponse CreateDto() =>
        new(
            Id,
            FirstName,
            LastName,
            FirstName + ' ' + LastName,
            Gender,
            ImageUrl
        );
}

public enum Gender
{
    Female,
    Male,
    Other
}