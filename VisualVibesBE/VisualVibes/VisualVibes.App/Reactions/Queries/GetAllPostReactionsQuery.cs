using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Reactions.Queries
{
    public record GetAllPostReactionsQuery(Guid PostId) : IRequest<ICollection<ReactionDto>>;
}
