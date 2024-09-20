using FluentValidation;
using ICorteApi.Domain.Interfaces;

namespace ICorteApi.Application.Services;

public sealed class MessageService(
    IMessageRepository repository,
    IValidator<MessageDtoCreate> createValidator,
    IValidator<MessageDtoIsReadUpdate> updateValidator,
    IMessageErrors errors)
    : BaseService<Message>(repository), IMessageService
{
    new private readonly IMessageRepository _repository = repository;
    private readonly IValidator<MessageDtoCreate> _createValidator = createValidator;
    private readonly IValidator<MessageDtoIsReadUpdate> _updateValidator = updateValidator;
    private readonly IMessageErrors _errors = errors;

    public async Task<bool> CanSendMessageAsync(int appointmentId, int userId)
    {
        return await _repository.CanSendMessageAsync(appointmentId, userId);
    }

    public async Task<MessageDtoResponse> CreateAsync(MessageDtoCreate dto, int appointmentId, int senderId)
    {
        dto.CheckAndThrowExceptionIfInvalid(_createValidator, _errors);
        var message = new Message(dto, appointmentId, senderId);
        return (await CreateAsync(message))!.CreateDto();
    }

    public async Task<MessageDtoResponse> GetByIdAsync(int id, int appointmentId)
    {
        var message = await GetByIdAsync(id);

        if (message is null)
            _errors.ThrowNotFoundException();

        if (message!.AppointmentId != appointmentId)
            _errors.ThrowMessageNotBelongsToAppointmentException(appointmentId);

        return message.CreateDto();
    }

    public async Task<PaginationResponse<MessageDtoResponse>> GetAllAsync(int? page, int? pageSize, int appointmentId)
    {
        var response = await GetAllAsync(new(page, pageSize, x => x.AppointmentId == appointmentId, new(x => x.SentAt)));
        
        return new(
            [..response.Data.Select(service => service.CreateDto())],
            response.TotalItems,
            response.TotalPages,
            response.Page,
            response.PageSize
        );
    }

    public async Task<Message?> SendMessageAsync(MessageDtoCreate dtoRequest, int appointmentId, int senderId)
    {
        if (!await _repository.CanSendMessageAsync(appointmentId, senderId))
            _errors.ThrowNotAllowedToSendMessageException(senderId);

        var entity = new Message(dtoRequest, appointmentId, senderId);
        return await CreateAsync(entity);
    }

    public async Task<bool> MarkMessageAsReadAsync(MessageDtoIsReadUpdate[] dtos, int senderId)
    {
        foreach (var dto in dtos)
            dto.CheckAndThrowExceptionIfInvalid(_updateValidator, _errors);
        
        var ids = dtos.Where(dto => dto.IsRead).Select(dto => dto.Id).ToArray();
        return await _repository.MarkMessageAsReadAsync(ids, senderId);
    }
    
    public async Task<bool> DeleteAsync(int id, int appointmentId, int senderId)
    {
        var message = await GetByIdAsync(id);

        if (message is null)
            _errors.ThrowNotFoundException();

        if (message!.AppointmentId != appointmentId)
            _errors.ThrowMessageNotBelongsToAppointmentException(appointmentId);

        if (message.SenderId != senderId)
            _errors.ThrowMessageNotBelongsToSenderException(senderId);

        return await DeleteAsync(message);
    }

    public async Task<MessageDtoResponse[]> GetLastMessagesAsync(int appointmentId, int senderId, int? lastMessageId)
    {
        return await _repository.GetLastMessagesAsync(appointmentId, senderId, lastMessageId);
    }
    
    public async Task<ChatWithMessagesDtoResponse[]> GetChatHistoryAsync(int senderId, bool isBarber)
    {
        return await _repository.GetChatHistoryAsync(senderId, isBarber);
    }

    // public async Task<List<Message>> GetConversationAsync(int barberId, int clientId);
    // public async Task<List<Message>> GetUnreadMessagesAsync(int barberId, int clientId);
    // public async Task<bool> IsActiveConversationAsync(int barberId, int clientId);
    // public async Task<Conversation> StartConversationAsync(int clientId, int barberId);
}


/*
1. Enviar Mensagem
Nome: SendMessageAsync
Descrição: Envia uma mensagem de um usuário para outro, validando as regras de envio.
Parâmetros: int senderId, int recipientId, string messageText
Regras de Negócio:
Verificar se o remetente é o Barber e, se for, garantir que o Client já enviou uma mensagem ou existe um Appointment ativo entre eles.
Se o remetente for o Client, a mensagem é enviada sem restrições.
Retorno: Task<IResponse> (ou outro tipo adequado)

2. Obter Conversa Entre Cliente e Barbeiro
Nome: GetConversationAsync
Descrição: Retorna todas as mensagens trocadas entre um Barber e um Client.
Parâmetros: int barberId, int clientId
Retorno: Task<List<Message>>

3. Verificar Elegibilidade para Enviar Mensagem
Nome: CanBarberSendMessageAsync
Descrição: Verifica se um Barber pode enviar uma mensagem a um Client.
Parâmetros: int barberId, int clientId
Regras de Negócio:
O Client deve ter iniciado a conversa ou existir um Appointment ativo entre eles.
Retorno: Task<bool>

4. Obter Mensagens Não Lidas
Nome: GetUnreadMessagesAsync
Descrição: Retorna todas as mensagens não lidas de uma conversa entre um Barber e um Client.
Parâmetros: int barberId, int clientId
Retorno: Task<List<Message>>

5. Marcar Mensagem como Lida
Nome: MarkMessageAsReadAsync
Descrição: Marca uma mensagem como lida.
Parâmetros: int messageId
Retorno: Task<IResponse>

6. Excluir Mensagem
Nome: DeleteMessageAsync
Descrição: Exclui uma mensagem de uma conversa.
Parâmetros: int messageId, int userId
Regras de Negócio:
Somente o remetente pode excluir a mensagem.
Retorno: Task<IResponse>

7. Obter Histórico de Conversas do Cliente
Nome: GetClientChatHistoryAsync
Descrição: Retorna o histórico de conversas do Client com todos os Barbers com os quais ele interagiu.
Parâmetros: int clientId
Retorno: Task<List<Conversation>> (ou outro tipo que represente uma lista de conversas)

8. Obter Histórico de Conversas do Barbeiro
Nome: GetBarberChatHistoryAsync
Descrição: Retorna o histórico de conversas do Barber com todos os Clients com os quais ele interagiu.
Parâmetros: int barberId
Retorno: Task<List<Conversation>> (ou outro tipo que represente uma lista de conversas)

9. Verificar se Existe Conversa Ativa
Nome: IsActiveConversationAsync
Descrição: Verifica se existe uma conversa ativa (não finalizada) entre um Barber e um Client.
Parâmetros: int barberId, int clientId
Retorno: Task<bool>

10. Iniciar Conversa
Nome: StartConversationAsync
Descrição: Inicia uma nova conversa entre um Barber e um Client, geralmente após o Client enviar a primeira mensagem ou abrir um Appointment.
Parâmetros: int clientId, int barberId
Retorno: Task<Conversation> (ou outro tipo que represente a conversa iniciada)
*/
