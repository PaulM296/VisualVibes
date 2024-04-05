using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Messages.Commands
{
    public record CreateMessageCommand(MessageDto MessageDto) : IRequest<MessageDto>;
}
