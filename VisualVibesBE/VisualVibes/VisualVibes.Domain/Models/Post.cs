namespace VisualVibes.Domain.Models.BaseEntity
{
    public class Post : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Caption { get; set; }
        public string Pictures { get; set; }
        public List<Reaction> Reactions { get; set; }
        public List<Comment> Comment {  get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
