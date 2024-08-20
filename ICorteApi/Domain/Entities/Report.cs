using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Enums;

namespace ICorteApi.Domain.Entities;

public sealed class Report : BasePrimaryKeyEntity<Report, int>
{
    public string? Title { get; private set; }
    public string? Content { get; private set; }
    public Rating Rating { get; private set; }

    public int ClientId { get; init; }
    public User Client { get; init; }

    public int BarberShopId { get; init; }
    public BarberShop BarberShop { get; init; }

    private Report() {}

    public Report(ReportDtoRequest dto, int? clientId = null, int? barberShopId = null)
    {
        Title = dto.Title;
        Content = dto.Content;
        Rating = dto.Rating;

        ClientId = clientId ?? default;
        BarberShopId = barberShopId ?? default;
    }

    private void UpdateByReportDto(ReportDtoRequest dto, DateTime? utcNow)
    {
        utcNow ??= DateTime.UtcNow;

        Title = dto.Title;
        Content = dto.Content;
        Rating = dto.Rating;
        
        UpdatedAt = utcNow;
    }

    public override void UpdateEntityByDto(IDtoRequest<Report> requestDto, DateTime? utcNow = null)
    {
        switch (requestDto)
        {
            case ReportDtoRequest dto:
                UpdateByReportDto(dto, utcNow);
                break;
            default:
                throw new ArgumentException("Tipo de DTO inválido", nameof(requestDto));
        }
    }

    public override ReportDtoResponse CreateDto() =>
        new(
            Id,
            Title,
            Content,
            Rating
        );
}
