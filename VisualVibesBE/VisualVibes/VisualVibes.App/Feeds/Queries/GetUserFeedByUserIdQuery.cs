using MediatR;
using VisualVibes.App.DTOs.FeedDtos;
using VisualVibes.App.DTOs.PaginationDtos;

namespace VisualVibes.App.Feeds.Queries
{
    public record GetUserFeedByUserIdQuery(string userId, PaginationRequestDto paginationRequestDto) : IRequest<PaginationResponseDto<ResponseFeedDto>>;
}
