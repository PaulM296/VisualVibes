using MediatR;
using VisualVibes.App.DTOs.FeedDtos;

namespace VisualVibes.App.Feeds.Commands
{
    public record CreateFeedCommand(CreateFeedDto FeedDto) : IRequest<Unit>;
}
