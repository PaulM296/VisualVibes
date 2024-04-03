using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Comments.Commands
{
    public record CreateCommentCommand(CommentDto CommentDto) : IRequest<CommentDto>;
}
