using MediatR;
using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.App.DTOs.ReactionDtos;

namespace VisualVibes.App.Reactions.Queries
{
    public record GetAllPostReactionsQuery(Guid PostId, PaginationRequestDto paginationRequestDto) : IRequest<PaginationResponseDto<ResponseReactionDto>>;
}
