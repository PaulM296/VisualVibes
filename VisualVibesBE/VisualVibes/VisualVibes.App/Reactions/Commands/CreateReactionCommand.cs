using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Reactions.Commands
{
    public record CreateReactionCommand(ReactionDto ReactionDto) : IRequest<ReactionDto>;
}
