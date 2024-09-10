using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Domain.Entities;

public sealed class Report : BaseEntity<Report>
{
    public string? Title { get; private set; }
    public string? Content { get; private set; }
    public int Rating { get; private set; }

    public int ClientId { get; init; }
    public User Client { get; init; }

    public int BarberShopId { get; init; }
    public BarberShop BarberShop { get; init; }

    private Report() { }

    public Report(ReportDtoCreate dto, int? clientId = null, int? barberShopId = null)
    {
        Title = GetValidStringOrNull(dto.Title);
        Content = GetValidStringOrNull(dto.Content);
        Rating = GetValidRatingOrNull(dto.Rating);

        ClientId = clientId ?? default;
        BarberShopId = barberShopId ?? default;
    }
    
    private void UpdateByReportDto(ReportDtoUpdate dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        Title = GetValidStringOrNull(dto.Title);
        Content = GetValidStringOrNull(dto.Content);
        Rating = GetValidRatingOrNull(dto.Rating);

        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<Report> requestDto, DateTime? utcNow = null)
    {
        switch (requestDto)
        {
            case ReportDtoUpdate dto:
                UpdateByReportDto(dto, utcNow);
                break;
            default:
                throw new ArgumentException("Tipo de DTO inválido", nameof(requestDto));
        }
    }

    public override ReportDtoResponse CreateDto() =>
        new(
            Id,
            BarberShopId,
            Title,
            Content,
            Rating
        );
}
