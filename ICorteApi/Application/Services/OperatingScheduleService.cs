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

    public async Task<IResponseModel> CreateAsync(int barberShopId, OperatingScheduleDtoRequest dto)
    {
        var newOperatingSchedule = dto.CreateEntity<OperatingSchedule>();
        newOperatingSchedule!.BarberShopId = barberShopId;
        
        return await _repository.CreateAsync(newOperatingSchedule);
    }

    public async Task<IResponseModel> CreateManyAsync(OperatingScheduleDtoRequest[] dtoArr)
    {
        var operatingScheduleArr = dtoArr.Select(dto => dto.CreateEntity<OperatingSchedule>()).ToArray();
        return await _repository.CreateManyAsync(operatingScheduleArr!);
    }

    public async Task<IResponseDataModel<ICollection<OperatingSchedule>>> GetAllAsync(int barberShopId)
    {
        return await _repository.GetAllAsync(barberShopId);
    }

    public async Task<IResponseDataModel<OperatingSchedule>> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        return await _repository.GetByIdAsync(dayOfWeek, barberShopId);
    }

    public async Task<IResponseModel> UpdateAsync(int barberShopId, OperatingScheduleDtoRequest dto)
    {
        var response = await _repository.GetByIdAsync(dto.DayOfWeek, barberShopId);

        if (!response.Success)
            return new ResponseModel(response.Success, "Horário de funcionamento não encontrado");
            
        var barberShop = response.Data;

        barberShop.UpdateEntityByDto(dto);
        return await _repository.UpdateAsync(barberShop);
    }

    public async Task<IResponseModel> UpdateManyAsync(int barberShopId, OperatingScheduleDtoRequest[] dtoArr)
    {
        var response = await _repository.GetAllAsync(barberShopId);

        if (!response.Success)
            return new ResponseModel(response.Success, "Horários de funcionamento não encontrados");
            
        var operatingSchedules = response.Data?.ToArray();
        
        foreach (var operatingSchedule in operatingSchedules)
        {
            var dto = dtoArr.FirstOrDefault(dto => dto.DayOfWeek == operatingSchedule.DayOfWeek);

            if (dto is not null)
                operatingSchedule.UpdateEntityByDto(dto);
        }
        
        return await _repository.UpdateManyAsync(operatingSchedules);
    }

    public async Task<IResponseModel> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        var response = await _repository.GetByIdAsync(dayOfWeek, barberShopId);

        if (!response.Success)
            return response;
            
        return await _repository.DeleteAsync(response.Data);
    }
}
