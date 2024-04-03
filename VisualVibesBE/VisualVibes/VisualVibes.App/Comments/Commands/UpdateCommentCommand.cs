using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Comments.Commands
{
    public record UpdateCommentCommand(CommentDto CommentDto) : IRequest<CommentDto>;
}
