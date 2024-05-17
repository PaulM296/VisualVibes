using MediatR;

namespace VisualVibes.App.DTOs.CommentDtos
{
    public record CreateCommentDto
    {
        public string UserId { get; set; }
        public Guid PostId { get; set; }
        public string Text { get; set; }
    }
}
