using ICorteApi.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICorteApi.Infraestructure.Maps;

public class ConversationMap : BaseMap<Conversation>
{
    public override void Configure(EntityTypeBuilder<Conversation> builder)
    {
        base.Configure(builder);

        // builder.HasOne(m => m.Conversation)
        //     .WithMany(c => c.Messages)
        //     .HasForeignKey(m => m.ConversationId);

        // builder.HasOne(m => m.Sender)
        //     .WithMany(s => s.Messages)
        //     .HasForeignKey(m => m.SenderId);
    }
}
