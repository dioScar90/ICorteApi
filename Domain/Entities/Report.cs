using ICorteApi.Application.Validators;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Errors;

namespace ICorteApi.Domain.Entities;

public sealed class Report : BaseEntity<Report, ReportDto>
{
    public string? Title { get; private set; }
    public string? Content { get; private set; }
    public int Rating { get; private set; }

    public int ClientId { get; init; }
    public User Client { get; init; }

    public int BarberShopId { get; init; }
    public BarberShop BarberShop { get; init; }

    private Report() { }

    public Report(ReportDto dto, int? clientId = null, int? barberShopId = null)
    {
        dto.ThrowExceptionIfInvalid(new ReportDtoValidator(), new ReportErrors());

        Title = GetValidStringOrNull(dto.Title);
        Content = GetValidStringOrNull(dto.Content);
        Rating = GetValidRatingOrNull(dto.Rating);

        ClientId = clientId ?? default;
        BarberShopId = barberShopId ?? default;
    }
    
    public override void UpdateEntityByDto(ReportDto dto, DateTime? utcNow = null)
    {
        utcNow ??= DateTime.UtcNow;

        Title = GetValidStringOrNull(dto.Title);
        Content = GetValidStringOrNull(dto.Content);
        Rating = GetValidRatingOrNull(dto.Rating);

        UpdatedAt = utcNow;
    }

    public override ReportDto CreateDto() => new(
        Id,
        BarberShopId,
        Title,
        Content,
        Rating
    );
}
