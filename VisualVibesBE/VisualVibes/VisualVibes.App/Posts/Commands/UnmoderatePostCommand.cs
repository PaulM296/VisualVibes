using MediatR;

namespace VisualVibes.App.Posts.Commands
{
    public record UnmoderatePostCommand(Guid postId) : IRequest<Unit>;
}
