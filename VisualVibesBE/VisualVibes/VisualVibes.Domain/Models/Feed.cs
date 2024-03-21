namespace VisualVibes.Domain.Models
{
    public class Feed : BaseEntity
    {
        public List<Post> Posts { get; set; }
    }
}
