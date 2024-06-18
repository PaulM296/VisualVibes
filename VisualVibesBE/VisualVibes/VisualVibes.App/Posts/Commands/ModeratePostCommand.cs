using MediatR;

namespace VisualVibes.App.Posts.Commands
{
    public record ModeratePostCommand(Guid postId) : IRequest<Unit>;
}
