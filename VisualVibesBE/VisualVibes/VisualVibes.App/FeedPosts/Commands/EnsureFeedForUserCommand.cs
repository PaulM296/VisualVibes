using MediatR;

namespace VisualVibes.App.FeedPosts.Commands
{
    public record EnsureFeedForUserCommand(string userId) : IRequest<Unit>;
}
