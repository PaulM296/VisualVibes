using MediatR;
using VisualVibes.App.DTOs.ReactionDtos;

namespace VisualVibes.App.Reactions.Commands
{
    public record UpdateReactionCommand(string userId, Guid id, UpdateReactionDto updateReactionDto) : IRequest<ResponseReactionDto>;
}
