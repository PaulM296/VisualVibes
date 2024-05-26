using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Configurations
{
    public class UserProfileConfiguration : IEntityTypeConfiguration<UserProfile>
    {
        public void Configure(EntityTypeBuilder<UserProfile> builder)
        {
            builder
                .HasOne(up => up.Image)
                .WithOne()
                .HasForeignKey<UserProfile>(up => up.ImageId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
