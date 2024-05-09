using MediatR;
using VisualVibes.App.DTOs.MessageDtos;

namespace VisualVibes.App.Messages.Queries
{
    public record GetAllConversationMessagesQuery(Guid ConversationId) : IRequest<ICollection<ResponseMessageDto>>;
}
