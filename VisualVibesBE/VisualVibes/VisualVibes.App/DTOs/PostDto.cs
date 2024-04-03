using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Caption { get; set; }
        public string Pictures { get; set; }
        public List<Reaction> Reactions { get; set; }
        public List<Comment> Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public static PostDto FromPost(Post post)
        {
            return new PostDto
            {
                Id = post.Id,
                UserId = post.UserId,
                Caption = post.Caption,
                Pictures = post.Pictures,
                Reactions = post.Reactions,
                Comment = post.Comment,
                CreatedAt = post.CreatedAt
            };
        }
    }
}
