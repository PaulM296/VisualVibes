using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Conversations.Commands
{
    public record CreateConversationCommand(ConversationDto ConversationDto) : IRequest<ConversationDto>;
}
