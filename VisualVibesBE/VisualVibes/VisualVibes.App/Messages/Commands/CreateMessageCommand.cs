using MediatR;
using VisualVibes.App.DTOs.MessageDtos;

namespace VisualVibes.App.Messages.Commands
{
    public record CreateMessageCommand(CreateMessageDto createMessageDto) : IRequest<ResponseMessageDto>;
}
