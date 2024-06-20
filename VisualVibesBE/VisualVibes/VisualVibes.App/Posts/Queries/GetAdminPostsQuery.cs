using MediatR;
using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.App.DTOs.PostDtos;

namespace VisualVibes.App.Posts.Queries
{
    public record GetAdminPostsQuery(PaginationRequestDto paginationRequest) : IRequest<PaginationResponseDto<JoinedResponsePostDto>>;
}
