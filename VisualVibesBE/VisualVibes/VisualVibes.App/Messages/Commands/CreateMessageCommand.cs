using MediatR;
using VisualVibes.App.DTOs.MessageDtos;

namespace VisualVibes.App.Messages.Commands
{
    public record CreateMessageCommand(string userId, CreateMessageDto createMessageDto) : IRequest<ResponseMessageDto>;
}
