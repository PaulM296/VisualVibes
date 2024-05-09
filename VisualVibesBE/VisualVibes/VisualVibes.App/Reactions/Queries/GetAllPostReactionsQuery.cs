using MediatR;
using VisualVibes.App.DTOs.ReactionDtos;

namespace VisualVibes.App.Reactions.Queries
{
    public record GetAllPostReactionsQuery(Guid PostId) : IRequest<ICollection<ResponseReactionDto>>;
}
