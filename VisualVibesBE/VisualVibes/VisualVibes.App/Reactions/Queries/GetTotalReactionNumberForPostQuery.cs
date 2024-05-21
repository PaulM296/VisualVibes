using MediatR;

namespace VisualVibes.App.Reactions.Queries
{
    public record GetTotalReactionNumberForPostQuery(Guid postId) : IRequest<int>; 
}
