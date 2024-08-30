using ICorteApi.Application.Dtos;
using ICorteApi.Domain.Entities;
using ICorteApi.Infraestructure.Context;
using ICorteApi.Infraestructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ICorteApi.Infraestructure.Repositories;

public sealed class MessageRepository(AppDbContext context)
    : BaseRepository<Message>(context), IMessageRepository
{
    public async Task<bool> MarkMessageAsReadAsync(int[] messageIds, int senderId)
    {
        var changesCount = await _dbSet
            .Where(x => !x.IsRead && messageIds.Contains(x.Id) && x.SenderId == senderId)
            .ExecuteUpdateAsync(x => x.SetProperty(p => p.IsRead, true));

        return changesCount > 0;
    }
    
    public async Task<MessageDtoResponse[]> GetLastMessagesAsync(int appointmentId, int senderId, int? lastMessageId)
    {
        int take = lastMessageId is int ? 10 : 5;

        return await _context.Database
            .SqlQuery<MessageDtoResponse>(@$"
                SELECT TOP ({take}) m.id AS Id
                    ,m.content AS Content
                    ,m.sent_at AS SentAt
                    ,m.is_read AS IsRead
                    ,m.sender_id AS SenderId
                    ,p.first_name AS FirstName
                    ,p.last_name AS LastName
                FROM messages m
                    INNER JOIN appointments a ON a.id m.appointment_id
                    INNER JOIN profiles p ON p.id = m.sender_id
                WHERE m.is_deleted = 0
                    AND a.id = {appointmentId}
                    AND (
                        a.client_id = {senderId}
                        OR
                        a.barber_shop_id = (
                            SELECT bs.id
                            FROM barber_shops bs
                            WHERE bs.owner_id = {senderId}
                        )
                    )
                    AND ({lastMessageId} IS NULL OR m.id > {lastMessageId})
                ORDER BY m.sent_at DESC
            ")
            .AsNoTrackingWithIdentityResolution()
            .ToArrayAsync();

        /*
            SELECT 
        */

        // var messages = await _dbSet.AsNoTrackingWithIdentityResolution()
        //     .OrderByDescending(x => x.SentAt)
        //     .Where(x => x.AppointmentId == appointmentId && (lastMessageId == null || x.Id > lastMessageId))
        //     .Take(take)
        //     .ToListAsync();

        // if (messages is null)
        //     return Response.FailureCollection<Message>(Error.TEntityNotFound);
        
        // return Response.Success(messages);
    }
}
