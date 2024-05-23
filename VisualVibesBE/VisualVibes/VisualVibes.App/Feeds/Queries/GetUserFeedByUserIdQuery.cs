using MediatR;
using VisualVibes.App.DTOs.FeedDtos;

namespace VisualVibes.App.Feeds.Queries
{
    public record GetUserFeedByUserIdQuery(string userId) : IRequest<ResponseFeedDto>;
}
