using ICorteApi.Application.Dtos;
using ICorteApi.Application.Interfaces;
using ICorteApi.Domain.Entities;
using ICorteApi.Domain.Interfaces;
using ICorteApi.Infraestructure.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class MessageService(IMessageRepository repository)
    : BaseService<Message>(repository), IMessageService
{
    public async Task<ISingleResponse<Message>> CreateAsync(IDtoRequest<Message> dtoRequest, int appointmentId, int senderId)
    {
        if (dtoRequest is not MessageDtoRequest dto)
            throw new ArgumentException("Tipo de DTO inválido", nameof(dtoRequest));

        var entity = new Message(dto, appointmentId, senderId);
        return await CreateAsync(entity);
    }

    public async Task<ISingleResponse<Message>> GetByIdAsync(int id, int appointmentId)
    {
        return await GetByIdAsync(x => x.Id == id && x.AppointmentId == appointmentId);
    }
    
    public async Task<ICollectionResponse<Message>> GetAllAsync(int? page, int? pageSize, int appointmentId)
    {
        return await GetAllAsync(new(page, pageSize, x => x.AppointmentId == appointmentId));
    }
    
    public async Task<IResponse> DeleteAsync(int id, int appointmentId)
    {
        var resp = await GetByIdAsync(id, appointmentId);

        if (!resp.IsSuccess)
            return resp;

        var entity = resp.Value!;
        return await DeleteAsync(entity);
    }
}
