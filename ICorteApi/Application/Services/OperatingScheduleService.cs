using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public class OperatingScheduleService(IOperatingScheduleRepository operatingScheduleRepository) : IOperatingScheduleService
{
    private readonly IOperatingScheduleRepository _repository = operatingScheduleRepository;

    public async Task<IResponseDataModel<OperatingSchedule>> CreateAsync(int barberShopId, OperatingScheduleDtoRequest dto)
    {
        var newOperatingSchedule = dto.CreateEntity<OperatingSchedule>();
        newOperatingSchedule!.BarberShopId = barberShopId;
        var response = await _repository.CreateAsync(newOperatingSchedule);

        if (!response.Success)
            return new ResponseDataModel<OperatingSchedule> { Success = false };
        
        return new ResponseDataModel<OperatingSchedule> { Success = true, Data = newOperatingSchedule };
    }

    public async Task<IResponseModel> CreateManyAsync(OperatingScheduleDtoRequest[] dtoArr)
    {
        var operatingScheduleArr = dtoArr.Select(dto => dto.CreateEntity<OperatingSchedule>()).ToArray();
        return await _repository.CreateManyAsync(operatingScheduleArr!);
    }

    public async Task<IResponseDataModel<IEnumerable<OperatingSchedule>>> GetAllAsync(int barberShopId)
    {
        return await _repository.GetAllAsync(barberShopId);
    }

    public async Task<IResponseDataModel<OperatingSchedule>> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        return await _repository.GetByIdAsync(dayOfWeek, barberShopId);
    }

    public async Task<IResponseModel> UpdateAsync(int barberShopId, OperatingScheduleDtoRequest dto)
    {
        var operatingSchedule = dto.CreateEntity<OperatingSchedule>();
        operatingSchedule!.BarberShopId = barberShopId;

        return await _repository.CreateAsync(operatingSchedule);
    }

    public async Task<IResponseModel> UpdateManyAsync(int barberShopId, OperatingScheduleDtoRequest[] dtoArr)
    {
        var operatingScheduleArr = dtoArr
            .Select(dto =>
                {
                    var newOperatingSchedule = dto.CreateEntity<OperatingSchedule>();
                    newOperatingSchedule!.BarberShopId = barberShopId;
                    return newOperatingSchedule;
                })
            .ToArray();
        return await _repository.CreateManyAsync(operatingScheduleArr!);
    }

    public async Task<IResponseModel> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        var response = await _repository.GetByIdAsync(dayOfWeek, barberShopId);

        if (!response.Success)
            return response;

        return await _repository.DeleteAsync(response.Data);
    }
}
