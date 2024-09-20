using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class MessageRepository(AppDbContext context)
    : BaseRepository<Message>(context), IMessageRepository
{
    public async Task<bool> CanSendMessageAsync(int appointmentId, int userId)
    {
        // var result = await _context.Appointments.AnyAsync(
        //     a => a.Id == appointmentId && (
        //         a.ClientId == userId || (
        //             a.BarberShop.OwnerId == userId
        //             && a.Messages.Any(m => m.AppointmentId == a.Id && m.SenderId == a.ClientId)
        //         )
        //     )
        // );

        var result = await _context.Database
            .SqlQuery<CanSend>(@$"
                SELECT
                    CASE
                        WHEN EXISTS (
                            SELECT 1
                            FROM appointments A
                            WHERE A.is_deleted = CAST(0 AS bit)
                                AND A.id = {appointmentId}
                                AND (
                                    A.client_id = {userId} 
                                    OR (
                                        A.barber_shop_id = (
                                            SELECT BS.id
                                            FROM barber_shops BS
                                            WHERE BS.is_deleted = CAST(0 AS bit)
                                                AND BS.owner_id = {userId}
                                        )
                                        AND EXISTS (
                                            SELECT 1
                                            FROM messages M
                                            WHERE M.appointment_id = A.id
                                                AND M.sender_id = A.client_id
                                        )
                                    )
                                )
                        )
                        THEN CAST(1 AS bit)
                        ELSE CAST(0 AS bit)
                    END AS IsAllowed
            ").SingleAsync();

        return result.IsAllowed;
    }

    public async Task<bool> MarkMessageAsReadAsync(int[] messageIds, int senderId)
    {
        var changesCount = await _dbSet
            .Where(x => !x.IsRead && messageIds.Contains(x.Id) && x.SenderId == senderId)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsRead, true));

        return changesCount > 0;
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
                        SELECT bs.id
                        FROM barber_shops bs
                        WHERE BS.is_deleted = CAST(0 AS bit)
                            AND bs.owner_id = {ownerBarberShopId}
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
                            SELECT bs.id
                            FROM barber_shops bs
                            WHERE BS.is_deleted = CAST(0 AS bit)
                                AND bs.owner_id = {senderId}
                        )
                    )
                    AND ({lastMessageId} IS NULL OR M.id > {lastMessageId})
                ORDER BY M.sent_at DESC
            ")
            .AsNoTracking()
            .ToArrayAsync();
    }

    internal record CanSend(bool IsAllowed);
}
