using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class ConversationParticipantMap : BaseMap<ConversationParticipant>
{
    public override void Configure(EntityTypeBuilder<ConversationParticipant> builder)
    {
        base.Configure(builder);

        builder.HasKey(pcv => new { pcv.ParticipantId, pcv.ConversationId });

        builder.HasOne(pcv => pcv.Conversation)
            .WithMany(c => c.ConversationParticipants)
            .HasForeignKey(pcv => pcv.ConversationId);

        builder.HasOne(pcv => pcv.Participant)
            .WithMany()
            .HasForeignKey(pcv => pcv.ParticipantId);
    }
}
