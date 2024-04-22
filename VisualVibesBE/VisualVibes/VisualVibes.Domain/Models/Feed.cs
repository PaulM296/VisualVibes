namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Feed : BaseEntity
    {
        public Guid UserID { get; set; }
        
        public User User { get; set; }

        public ICollection<FeedPost> FeedPosts { get; set; }
    }
}
