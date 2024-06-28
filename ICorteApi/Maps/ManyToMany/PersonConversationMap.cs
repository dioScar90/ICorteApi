using ICorteApi.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Maps;

public class PersonConversationMap : BaseMap<PersonConversation>
{
    public override void Configure(EntityTypeBuilder<PersonConversation> builder)
    {
        base.Configure(builder);
        
        builder.HasKey(pcv => new { pcv.ParticipantId, pcv.ConversationId });
        
        builder.HasOne(pcv => pcv.Participant)
            .WithMany(p => p.PersonConversations)
            .HasForeignKey(pcv => pcv.ParticipantId);
        
        builder.HasOne(pcv => pcv.Conversation)
            .WithMany(c => c.PersonConversations)
            .HasForeignKey(pcv => pcv.ConversationId);
    }
}
