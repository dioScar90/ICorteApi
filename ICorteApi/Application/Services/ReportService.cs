using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class ReportService(IReportRepository repository, IReportErrors errors)
    : BaseService<Report>(repository), IReportService
{
    new private readonly IReportRepository _repository = repository;
    private readonly IReportErrors _errors = errors;

    public async Task<Report?> CreateAsync(ReportDtoRequest dto, int clientId, int barberShopId)
    {
        var report = new Report(dto, clientId, barberShopId);
        return await CreateAsync(report);
    }

    public async Task<Report?> GetByIdAsync(int id, int clientId, int barberShopId)
    {
        var report = await GetByIdAsync(id);
        
        if (report is null)
            _errors.ThrowNotFoundException();

        if (report!.ClientId != clientId)
            _errors.ThrowReportNotBelongsToClientException(clientId);

        if (report.BarberShopId != barberShopId)
            _errors.ThrowReportNotBelongsToBarberShopException(barberShopId);

        return report;
    }
    
    public async Task<Report[]> GetAllAsync(int? page, int? pageSize, int barberShopId)
    {
        return await GetAllAsync(new(page, pageSize, x => x.BarberShopId == barberShopId));
    }
    
    public async Task<bool> UpdateAsync(ReportDtoRequest dtoRequest, int id, int clientId, int barberShopId)
    {
        var report = await _repository.GetReportWithBarberShopByIdAsync(id);
        
        if (report is null)
            _errors.ThrowNotFoundException();

        if (report!.ClientId != clientId)
            _errors.ThrowReportNotBelongsToClientException(clientId);

        if (report.BarberShopId != barberShopId)
            _errors.ThrowReportNotBelongsToBarberShopException(barberShopId);

        report.UpdateEntityByDto(dtoRequest);
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
