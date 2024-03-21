using System.Text.Json;

namespace VisualVibes.Domain.Models
{
    public class Post
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string Caption { get; set; }
        public string Pictures { get; set; }
        public List<Reaction> Reactions { get; set; }
        public List<Comment> Comment {  get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
