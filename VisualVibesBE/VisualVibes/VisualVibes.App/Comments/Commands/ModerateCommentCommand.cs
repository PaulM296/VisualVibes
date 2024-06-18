using MediatR;

namespace VisualVibes.App.Comments.Commands
{
    public record ModerateCommentCommand(Guid commentId) : IRequest<Unit>;
}
