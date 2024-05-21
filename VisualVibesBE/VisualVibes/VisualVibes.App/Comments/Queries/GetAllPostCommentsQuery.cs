using MediatR;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.DTOs.PaginationDtos;

namespace VisualVibes.App.Comments.Queries
{
    public record GetAllPostCommentsQuery(Guid PostId, PaginationRequestDto paginationRequestDto) : IRequest<PaginationResponseDto<ResponseCommentDto>>;
}
