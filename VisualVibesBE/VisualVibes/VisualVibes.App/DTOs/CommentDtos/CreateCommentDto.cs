using MediatR;

namespace VisualVibes.App.DTOs.CommentDtos
{
    public record CreateCommentDto
    {
        public Guid UserId { get; set; }
        public Guid PostId { get; set; }
        public string Text { get; set; }
    }
}
