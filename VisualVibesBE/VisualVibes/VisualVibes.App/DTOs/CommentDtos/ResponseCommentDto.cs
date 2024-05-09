using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.CommentDtos
{
    public class ResponseCommentDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }

        public static ResponseCommentDto FromComment(Comment comment)
        {
            return new ResponseCommentDto
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
