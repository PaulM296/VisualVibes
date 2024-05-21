using MediatR;
using VisualVibes.App.DTOs.ReactionDtos;

namespace VisualVibes.App.Reactions.Commands
{
    public record CreateReactionCommand(string userId, CreateReactionDto createReactionDto) : IRequest<ResponseReactionDto>;
}
