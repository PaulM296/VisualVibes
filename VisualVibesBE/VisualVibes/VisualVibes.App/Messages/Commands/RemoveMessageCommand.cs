using MediatR;

namespace VisualVibes.App.Messages.Commands
{
    public record RemoveMessageCommand(Guid Id) : IRequest<Unit>; 
}
