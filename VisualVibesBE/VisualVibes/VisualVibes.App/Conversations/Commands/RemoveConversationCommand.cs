using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Conversations.Commands
{
    public record RemoveConversationCommand(string userId, Guid Id) : IRequest<Unit>;
}
