using MediatR;

namespace VisualVibes.App.Messages.Commands
{
    public record RemoveMessagedCommand(Guid Id) : IRequest<Unit>; 
}
