using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Reactions.Queries
{
    public record GetAllPostReactionsCommand(Guid PostId) : IRequest<ICollection<ReactionDto>>;
}
