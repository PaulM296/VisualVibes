using Microsoft.EntityFrameworkCore;
using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;
using VisualVibes.Infrastructure.Configurations;

namespace VisualVibes.Infrastructure
{
    public class VisualVibesDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<UserFollower> UserFollower { get; set; }
        public DbSet<FeedPost> FeedPost { get; set; }

        public VisualVibesDbContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserFollowerConfiguration());
            modelBuilder.ApplyConfiguration(new ConversationConfiguration());
            modelBuilder.ApplyConfiguration(new CommentConfiguration());
            modelBuilder.ApplyConfiguration(new ReactionConfiguration());
            modelBuilder.ApplyConfiguration(new FeedPostConfiguration());
        }
    }
}
