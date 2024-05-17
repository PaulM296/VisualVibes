using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.CommentDtos
{
    public class ResponseCommentDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid PostId { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
