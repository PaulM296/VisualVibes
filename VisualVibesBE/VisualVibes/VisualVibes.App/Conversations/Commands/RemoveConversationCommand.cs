using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Conversations.Commands
{
    public record RemoveConversationCommand(Guid Id) : IRequest<Unit>;
}
