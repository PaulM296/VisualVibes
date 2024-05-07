using MediatR;

namespace VisualVibes.App.FeedPosts.Commands
{
    public record EnsureFeedForUserCommand(Guid userId) : IRequest<Unit>;
}
