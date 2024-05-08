using MediatR;
using VisualVibes.App.DTOs.ConversationDtos;

namespace VisualVibes.App.Conversations.Queries
{
    public record GetAllUserConversationsQuery(Guid UserId) : IRequest<ICollection<ResponseConversationDto>>;
}
