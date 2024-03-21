namespace VisualVibes.Domain.Models
{
    public class Feed
    {
        public Guid UserId { get; set; }
        public List<Post> Posts { get; set; }
    }
}
