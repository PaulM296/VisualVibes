using MediatR;
using VisualVibes.App.DTOs.MessageDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Messages.CommandsHandler
{
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, ResponseMessageDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateMessageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseMessageDto> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var message = new Message()
            {
                UserId = request.createMessageDto.UserId,
                ConversationId = request.createMessageDto.ConversationId,
                Content = request.createMessageDto.Content,
                Timestamp = request.createMessageDto.Timestamp
            };

            var createdMessage = await _unitOfWork.MessageRepository.AddAsync(message);
            await _unitOfWork.SaveAsync();

            return ResponseMessageDto.FromMessage(createdMessage);
        }
    }
}
