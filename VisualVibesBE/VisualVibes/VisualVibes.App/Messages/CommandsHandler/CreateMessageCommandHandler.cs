using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Messages.CommandsHandler
{
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, MessageDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<MessageDto> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var message = new Message()
            {
                Id = request.MessageDto.Id,
                UserId = request.MessageDto.UserId,
                ConversationId = request.MessageDto.ConversationId,
                Content = request.MessageDto.Content,
                Timestamp = request.MessageDto.Timestamp
            };

            var createdMessage = await _unitOfWork.MessageRepository.AddAsync(message);
            await _unitOfWork.SaveAsync();

            return MessageDto.FromMessage(createdMessage);
        }
    }
}
