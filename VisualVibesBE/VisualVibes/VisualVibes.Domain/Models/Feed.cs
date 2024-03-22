namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Feed : BaseEntity
    {
        public List<Post> Posts { get; set; }
    }
}
