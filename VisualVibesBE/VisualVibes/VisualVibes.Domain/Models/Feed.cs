namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Feed : BaseEntity
    {
        public string UserID { get; set; }
        
        public AppUser User { get; set; }

        public ICollection<FeedPost> FeedPosts { get; set; }
    }
}
