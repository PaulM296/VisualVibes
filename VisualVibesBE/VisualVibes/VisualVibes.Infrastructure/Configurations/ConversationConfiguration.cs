using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Configurations
{
    public class ConversationConfiguration : IEntityTypeConfiguration<Conversation>
    {
        public void Configure(EntityTypeBuilder<Conversation> builder)
        {
            builder
                .HasOne(c => c.FirstParticipant)
                .WithMany(u => u.StartedConversations)
                .HasForeignKey(c => c.FirstParticipantId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .HasOne(c => c.SecondParticipant)
                .WithMany(u => u.JoinedConversations)
                .HasForeignKey(c => c.SecondParticipantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
