using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Base;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;
using ICorteApi.Presentation.Extensions;

namespace ICorteApi.Application.Services;

public class RecurringScheduleService(IRecurringScheduleRepository recurringScheduleRepository) : IRecurringScheduleService
{
    private readonly IRecurringScheduleRepository _repository = recurringScheduleRepository;

    public async Task<IResponseModel> CreateAsync(int barberShopId, RecurringScheduleDtoRequest dto)
    {
        var newRecurringSchedule = dto.CreateEntity();
        newRecurringSchedule!.BarberShopId = barberShopId;

        return await _repository.CreateAsync(newRecurringSchedule);
    }

    public async Task<IResponseModel> CreateManyAsync(RecurringScheduleDtoRequest[] dtoArr)
    {
        var recurringScheduleArr = dtoArr.Select(dto => dto.CreateEntity()).ToArray();
        return await _repository.CreateManyAsync(recurringScheduleArr!);
    }

    public async Task<IResponseDataModel<ICollection<RecurringSchedule>>> GetAllAsync(int barberShopId)
    {
        return await _repository.GetAllAsync(barberShopId);
    }

    public async Task<IResponseDataModel<RecurringSchedule>> GetByIdAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        return await _repository.GetByIdAsync(dayOfWeek, barberShopId);
    }

    public async Task<IResponseModel> UpdateAsync(int barberShopId, RecurringScheduleDtoRequest dto)
    {
        var response = await _repository.GetByIdAsync(dto.DayOfWeek, barberShopId);

        if (!response.Success)
            return new ResponseModel(response.Success, "Horário de funcionamento não encontrado");

        var barberShop = response.Data;

        barberShop.UpdateEntityByDto(dto);
        return await _repository.UpdateAsync(barberShop);
    }

    public async Task<IResponseModel> UpdateManyAsync(int barberShopId, RecurringScheduleDtoRequest[] dtoArr)
    {
        var response = await _repository.GetAllAsync(barberShopId);

        if (!response.Success)
            return new ResponseModel(response.Success, "Horários de funcionamento não encontrados");

        var recurringSchedules = response.Data?.ToArray();

        foreach (var recurringSchedule in recurringSchedules)
        {
            var dto = dtoArr.FirstOrDefault(dto => dto.DayOfWeek == recurringSchedule.DayOfWeek);

            if (dto is not null)
                recurringSchedule.UpdateEntityByDto(dto);
        }

        return await _repository.UpdateManyAsync(recurringSchedules);
    }

    public async Task<IResponseModel> DeleteAsync(DayOfWeek dayOfWeek, int barberShopId)
    {
        var response = await _repository.GetByIdAsync(dayOfWeek, barberShopId);

        if (!response.Success)
            return response;

        return await _repository.DeleteAsync(response.Data);
    }
}
