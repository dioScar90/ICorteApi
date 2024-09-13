using FluentValidation;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ReportService(
    IReportRepository repository,
    IValidator<ReportDtoCreate> createValidator,
    IValidator<ReportDtoUpdate> updateValidator,
    IReportErrors errors)
    : BaseService<Report>(repository), IReportService
{
    new private readonly IReportRepository _repository = repository;
    private readonly IValidator<ReportDtoCreate> _createValidator = createValidator;
    private readonly IValidator<ReportDtoUpdate> _updateValidator = updateValidator;
    private readonly IReportErrors _errors = errors;

    public async Task<ReportDtoResponse> CreateAsync(ReportDtoCreate dto, int clientId, int barberShopId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_createValidator, _errors);
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
    
    public async Task<ReportDtoResponse[]> GetAllAsync(int? page, int? pageSize, int barberShopId)
    {
        var reports = await GetAllAsync(new(page, pageSize, x => x.BarberShopId == barberShopId));
        return [..reports.Select(report => report.CreateDto())];
    }
    
    public async Task<bool> UpdateAsync(ReportDtoUpdate dto, int id, int clientId, int barberShopId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_updateValidator, _errors);

        var report = await _repository.GetReportWithBarberShopByIdAsync(id);
        
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
        var report = await _repository.GetReportWithBarberShopByIdAsync(id);
        
        if (report is null)
            _errors.ThrowNotFoundException();

        if (report!.ClientId != clientId)
            _errors.ThrowReportNotBelongsToClientException(clientId);

        if (report.BarberShopId != barberShopId)
            _errors.ThrowReportNotBelongsToBarberShopException(barberShopId);
        
        return await DeleteAsync(report);
    }
}
