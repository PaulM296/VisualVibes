using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Conversations.Queries
{
    public record GetAllUserConversationsCommand : IRequest<ICollection<ConversationDto>>;
}
