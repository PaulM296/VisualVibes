using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Conversations.Queries
{
    public record GetAllUserConversationsQuery(Guid UserId) : IRequest<ICollection<ConversationDto>>;
}
