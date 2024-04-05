using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Messages.Queries
{
    public record GetAllConversationMessagesQuery(Guid ConversationId) : IRequest<ICollection<MessageDto>>;
}
