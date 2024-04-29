using MediatR;

namespace VisualVibes.App.FeedPosts.Commands
{
    public record AddPostToFeedCommand(Guid PostId) : IRequest<Unit>;
}
