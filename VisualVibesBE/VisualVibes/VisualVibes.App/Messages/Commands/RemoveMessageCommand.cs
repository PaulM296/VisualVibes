using MediatR;

namespace VisualVibes.App.Messages.Commands
{
    public record RemoveMessageCommand(string userId, Guid Id) : IRequest<Unit>; 
}
