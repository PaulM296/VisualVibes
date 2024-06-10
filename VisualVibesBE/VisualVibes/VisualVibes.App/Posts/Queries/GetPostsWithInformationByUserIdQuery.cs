using MediatR;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.App.DTOs.PostDtos;

namespace VisualVibes.App.Posts.Queries
{
    public record GetPaginatedPostsWithInformationByUserIdQuery(string userId, PaginationRequestDto paginationRequest) : IRequest<PaginationResponseDto<JoinedResponsePostDto>>;
}