using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Reactions.Commands
{
    public record UpdateReactionCommand(ReactionDto ReactionDto) : IRequest<ReactionDto>;
}
