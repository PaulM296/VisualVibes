using MediatR;
using VisualVibes.App.DTOs.ConversationDtos;

namespace VisualVibes.App.Conversations.Commands
{
    public record CreateConversationCommand(string userId, CreateConversationDto createConversationDto) : IRequest<ResponseConversationDto>;
}
