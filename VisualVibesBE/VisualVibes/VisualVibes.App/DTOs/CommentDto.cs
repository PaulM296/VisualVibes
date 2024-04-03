using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public static CommentDto FromComment(Comment comment)
        {
            return new CommentDto
            {
                Id = comment.Id,
                UserId = comment.UserId,
                PostId = comment.PostId,
                Text = comment.Text,
                CreatedAt = comment.CreatedAt
            };
        }
    }
}
