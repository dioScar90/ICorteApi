using FluentValidation;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ReportService(
    AppDbContext context,
    IValidator<ReportDtoCreate> createValidator,
    IValidator<ReportDtoUpdate> updateValidator,
    IReportErrors errors)
    : BaseService<Report>(context), IReportService
{
    private readonly IValidator<ReportDtoCreate> _createValidator = createValidator;
    private readonly IValidator<ReportDtoUpdate> _updateValidator = updateValidator;
    private readonly IReportErrors _errors = errors;

    public async Task<ReportDtoResponse> CreateAsync(ReportDtoCreate dto, int clientId, int barberShopId)
    {
        dto.ThrowExceptionIfInvalid(_createValidator, _errors);
        var report = new Report(dto, clientId, barberShopId);
        return (await CreateAsync(report))!.CreateDto();
    }

    public async Task<ReportDtoResponse> GetByIdAsync(int id, int clientId, int barberShopId)
    {
        var report = await GetByIdAsync(id);
        
        if (report is null)
            _errors.ThrowNotFoundException();

        if (report!.ClientId != clientId)
            _errors.ThrowReportNotBelongsToClientException(clientId);

        if (report.BarberShopId != barberShopId)
            _errors.ThrowReportNotBelongsToBarberShopException(barberShopId);

        return report.CreateDto();
    }
    
    public async Task<PaginationResponse<ReportDtoResponse>> GetAllAsync(int? page, int? pageSize, int barberShopId)
    {
        var response = await GetAllAsync(new(page, pageSize, x => x.BarberShopId == barberShopId, new(x => x.Id)));
        
        return new(
            [..response.Items.Select(service => service.CreateDto())],
            response.TotalItems,
            response.TotalPages,
            response.Page,
            response.PageSize
        );
    }

    private async Task<Report?> GetReportWithBarberShopByIdAsync(int id)
    {
        return await GetByIdAsync(x => x.Id == id, x => x.BarberShop);
    }
    
    public async Task<bool> UpdateAsync(ReportDtoUpdate dto, int id, int clientId, int barberShopId)
    {
        dto.ThrowExceptionIfInvalid(_updateValidator, _errors);

        var report = await GetReportWithBarberShopByIdAsync(id);
        
        if (report is null)
            _errors.ThrowNotFoundException();

        if (report!.ClientId != clientId)
            _errors.ThrowReportNotBelongsToClientException(clientId);

        if (report.BarberShopId != barberShopId)
            _errors.ThrowReportNotBelongsToBarberShopException(barberShopId);

        report.UpdateEntityByDto(dto);
        return await UpdateAsync(report);
    }

    public async Task<bool> DeleteAsync(int id, int clientId, int barberShopId)
    {
        var report = await GetReportWithBarberShopByIdAsync(id);
        
        if (report is null)
            _errors.ThrowNotFoundException();

        if (report!.ClientId != clientId)
            _errors.ThrowReportNotBelongsToClientException(clientId);

        if (report.BarberShopId != barberShopId)
            _errors.ThrowReportNotBelongsToBarberShopException(barberShopId);
        
        return await DeleteAsync(report);
    }
}
