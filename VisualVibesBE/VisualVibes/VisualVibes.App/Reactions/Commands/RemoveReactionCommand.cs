using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Reactions.Commands
{
    public record RemoveReactionCommand(string userId, Guid Id) : IRequest<Unit>;

}
