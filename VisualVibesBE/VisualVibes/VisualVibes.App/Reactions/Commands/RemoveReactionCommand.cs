using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Reactions.Commands
{
    public record RemoveReactionCommand(Guid Id) : IRequest<Unit>;

}
