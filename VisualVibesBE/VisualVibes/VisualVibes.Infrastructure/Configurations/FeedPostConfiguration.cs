using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;
using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Configurations
{
    public class FeedPostConfiguration : IEntityTypeConfiguration<FeedPost>
    {
        public void Configure(EntityTypeBuilder<FeedPost> builder)
        {
           builder
                .HasKey(fp => new { fp.FeedId, fp.PostId });

            builder
            .HasOne(fp => fp.Feed)
            .WithMany(f => f.FeedPosts)
            .HasForeignKey(fp => fp.FeedId)
            .OnDelete(DeleteBehavior.NoAction);

            builder
            .HasOne(fp => fp.Post)
            .WithMany(p => p.FeedPosts)
            .HasForeignKey(fp => fp.PostId)
            .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
