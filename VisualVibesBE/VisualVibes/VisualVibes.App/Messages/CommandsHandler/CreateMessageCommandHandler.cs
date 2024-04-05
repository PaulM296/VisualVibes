using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Messages.CommandsHandler
{
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, MessageDto>
    {
        private readonly IMessageRepository _messageRepository;
        public CreateMessageCommandHandler(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public async Task<MessageDto> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var message = new Message()
            {
                Id = request.MessageDto.Id,
                SenderId = request.MessageDto.SenderId,
                ReceiverId = request.MessageDto.ReceiverId,
                ConversationId = request.MessageDto.ConversationId,
                Content = request.MessageDto.Content,
                Timestamp = request.MessageDto.Timestamp
            };

            var createdMessage = await _messageRepository.AddAsync(message);
            return MessageDto.FromMessage(createdMessage);
        }
    }
}
