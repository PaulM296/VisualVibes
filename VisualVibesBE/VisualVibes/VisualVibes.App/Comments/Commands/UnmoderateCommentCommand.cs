using MediatR;

namespace VisualVibes.App.Comments.Commands
{
    public record UnmoderateCommentCommand(Guid commentId) : IRequest<Unit>;
}
