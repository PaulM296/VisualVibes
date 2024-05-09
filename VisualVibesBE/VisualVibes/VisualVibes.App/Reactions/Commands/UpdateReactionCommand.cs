using MediatR;
using VisualVibes.App.DTOs.ReactionDtos;

namespace VisualVibes.App.Reactions.Commands
{
    public record UpdateReactionCommand(Guid id, UpdateReactionDto updateReactionDto) : IRequest<ResponseReactionDto>;
}
