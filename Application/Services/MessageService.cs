using ICorteApi.Domain.Errors;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Application.Services;

public sealed class MessageService(
    AppDbContext context,
    ILogger<MessageService> _logger,
    UserService _userService,
    MessageErrors _errors)
    : BaseService<Message>(context)
{
    public async Task<bool> CanSendMessageAsync(int appointmentId, int? _userId = null)
    {
        var userId = _userId ?? await _userService.GetMyUserIdAsync()!;
        
        return await _context.Appointments.AnyAsync(
            a => a.Id == appointmentId && (
                a.ClientId == userId || (
                    a.BarberShop.OwnerId == userId
                    && a.Messages.Any(m => m.AppointmentId == a.Id && m.SenderId == a.ClientId)
                )
            )
        );
    }

    public async Task<MessageDtoResponse> CreateAsync(MessageDtoRequest dto)
    {
        var senderId = await _userService.GetMyUserIdAsync()!;
        var message = new Message(dto, dto.AppointmentId, senderId);
        
        _dbSet.Add(message);
        await SaveChangesAsync();

        return await GetByIdAsync(message.Id, message.AppointmentId);
    }

    public record Includes(bool Appointment = false);

    public async Task<MessageDtoResponse> GetByIdAsync(int id, int appointmentId, Includes? includes = null)
    {
        includes ??= new();

        var query = _dbSet
            .AsNoTracking()
            .Where(m => m.Id == id);

        if (includes.Appointment)
        {
            query = query
                .AsSplitQuery()
                .Include(m => m.Appointment);
        }

        var message = await query
            .Select(m => new MessageDtoResponse(
                m.Id,
                m.AppointmentId,
                m.SenderId,
                m.Content,
                m.SentAt,
                m.IsRead,
                m.Sender.Profile.FirstName,
                m.Sender.Profile.LastName
            ))
            .FirstOrDefaultAsync();

        if (message is null)
            _errors.ThrowNotFoundException();

        if (message!.AppointmentId != appointmentId)
            _errors.ThrowMessageNotBelongsToAppointmentException(appointmentId);

        return message;
    }

    public async Task<PaginationResponse<MessageDtoResponse>> GetAllAsync(int? page, int? pageSize, int appointmentId)
    {
        return await GetAllAsync<MessageDtoResponse>(
            new(
                page,
                pageSize,
                x => x.AppointmentId == appointmentId,
                new(x => x.SentAt),
                m => new(
                    m.Id,
                    m.AppointmentId,
                    m.SenderId,
                    m.Content,
                    m.SentAt,
                    m.IsRead,
                    m.Sender.Profile.FirstName,
                    m.Sender.Profile.LastName
                )
            )
        );
    }

    public async Task<MessageDtoResponse> SendMessageAsync(MessageDtoRequest dto, int appointmentId, int senderId)
    {
        if (!await CanSendMessageAsync(appointmentId, senderId))
            _errors.ThrowNotAllowedToSendMessageException(senderId);

        dto = dto with
        {
            AppointmentId = appointmentId,
            SenderId = senderId
        };

        return await CreateAsync(dto);
    }

    public async Task MarkMessageAsReadAsync(MessageDtoIsReadUpdateRequest[] dtos, int senderId)
    {
        var messageIds = dtos.Where(dto => dto.IsRead).Select(dto => dto.Id).ToArray();

        await _dbSet
            .Where(x => !x.IsRead && messageIds.Contains(x.Id) && x.SenderId == senderId)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsRead, true));
    }

    public async Task DeleteAsync(int id, int appointmentId)
    {
        var message = await _dbSet.FindAsync(id);

        if (message is null)
            _errors.ThrowNotFoundException();

        if (message!.AppointmentId != appointmentId)
            _errors.ThrowMessageNotBelongsToAppointmentException(appointmentId);
            
        var senderId = await _userService.GetMyUserIdAsync()!;
        
        if (message.SenderId != senderId)
            _errors.ThrowMessageNotBelongsToSenderException(senderId);

        await DeleteAsync(message);
    }

    public async Task<MessageDtoResponse[]> GetLastMessagesAsync(int appointmentId, int senderId, int? lastMessageId)
    {
        int take = lastMessageId is int ? 10 : 5;

        return await _context.Database
            .SqlQuery<MessageDtoResponse>(@$"
                SELECT TOP ({take}) M.id AS Id
                    ,M.content AS Content
                    ,M.sent_at AS SentAt
                    ,M.is_read AS IsRead
                    ,M.sender_id AS SenderId
                    ,P.first_name AS FirstName
                    ,P.last_name AS LastName
                FROM appointments A
                    INNER JOIN messages M ON A.id = M.appointment_id
                    INNER JOIN profiles P ON P.id = M.sender_id
                WHERE A.is_deleted = CAST(0 AS BIT)
                    AND A.id = {appointmentId}
                    AND M.is_deleted = CAST(0 AS BIT)
                    AND (
                        A.client_id = {senderId}
                        OR A.barber_shop_id = (
                            SELECT BS.id
                            FROM barber_shops BS
                            WHERE BS.is_deleted = CAST(0 AS bit)
                                AND BS.owner_id = {senderId}
                        )
                    )
                    AND ({lastMessageId} IS NULL OR M.id > {lastMessageId})
                ORDER BY M.sent_at DESC
            ")
            .AsNoTracking()
            .ToArrayAsync();
    }

    private async Task<ChatWithMessagesDtoResponse[]> GetClientChatHistoryAsync(int clientId)
    {
        return await _context.Database
            .SqlQuery<ChatWithMessagesDtoResponse>(@$"
                SELECT A.id AS AppointmentId
                    ,IIF(M.sender_id = {clientId}, CAST(1 AS BIT), CAST(0 AS BIT)) AS IsMe
                    ,M.content AS Content
                    ,M.sent_at AS SentAt
                    ,P.first_name AS FirstName
                    ,M.is_read AS IsRead
                FROM appointments A
                    INNER JOIN messages M ON A.id == M.appointment_id
                    INNER JOIN profiles P ON P.id = M.sender_id
                WHERE A.is_deleted = CAST(0 AS BIT)
                    AND M.is_deleted = CAST(0 AS BIT)
                    AND A.client_id = {clientId}
                    AND M.id = (
                        SELECT TOP 1 id
                        FROM messages MT
                        WHERE MT.appointment_id = A.id
                        ORDER BY M.sent_at DESC
                    )
                ORDER BY M.sent_at DESC
            ")
            .AsNoTracking()
            .ToArrayAsync();
    }

    private async Task<ChatWithMessagesDtoResponse[]> GetBarberChatHistoryAsync(int ownerBarberShopId)
    {
        return await _context.Database
            .SqlQuery<ChatWithMessagesDtoResponse>(@$"
                SELECT A.id AS AppointmentId
                    ,IIF(M.sender_id = {ownerBarberShopId}, CAST(1 AS BIT), CAST(0 AS BIT)) AS IsMe
                    ,M.content AS Content
                    ,M.sent_at AS SentAt
                    ,P.first_name AS FirstName
                    ,M.is_read AS IsRead
                FROM appointments A
                    INNER JOIN messages M ON A.id == M.appointment_id
                    INNER JOIN profiles P ON P.id = M.sender_id
                WHERE A.is_deleted = CAST(0 AS BIT)
                    AND M.is_deleted = CAST(0 AS BIT)
                    AND A.barber_shop_id = (
                        SELECT BS.id
                        FROM barber_shops BS
                        WHERE BS.is_deleted = CAST(0 AS bit)
                            AND BS.owner_id = {ownerBarberShopId}
                    )
                    AND M.id = (
                        SELECT TOP 1 id
                        FROM messages MT
                        WHERE MT.appointment_id = A.id
                            AND EXISTS (
                                SELECT 1
                                FROM messages MT2
                                WHERE MT2.sender_id == A.client_id
                            )
                        ORDER BY M.sent_at DESC
                    )
                ORDER BY M.sent_at DESC
            ")
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<ChatWithMessagesDtoResponse[]> GetChatHistoryAsync(int senderId, bool isBarber)
    {
        return isBarber ? await GetBarberChatHistoryAsync(senderId) : await GetClientChatHistoryAsync(senderId);
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
