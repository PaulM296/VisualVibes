using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Configurations
{
    public class PostConfiguration : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder
                .HasOne(p => p.Image)
                .WithOne()
                .HasForeignKey<Post>(p => p.ImageId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
