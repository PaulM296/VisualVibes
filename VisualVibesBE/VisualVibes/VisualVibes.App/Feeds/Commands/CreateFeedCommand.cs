using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Feeds.Commands
{
    public record CreateFeedCommand(FeedDto FeedDto) : IRequest<Unit>;
}
