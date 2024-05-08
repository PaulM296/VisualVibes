using MediatR;
using VisualVibes.App.DTOs.ConversationDtos;

namespace VisualVibes.App.Conversations.Commands
{
    public record CreateConversationCommand(CreateConversationDto createConversationDto) : IRequest<ResponseConversationDto>;
}
