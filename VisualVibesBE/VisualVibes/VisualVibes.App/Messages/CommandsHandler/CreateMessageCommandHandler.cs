using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.MessageDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Messages.CommandsHandler
{
    public class CreateMessageCommandHandler : IRequestHandler<CreateMessageCommand, ResponseMessageDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateMessageCommandHandler> _logger;
        private readonly IMapper _mapper;
        public CreateMessageCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateMessageCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseMessageDto> Handle(CreateMessageCommand request, CancellationToken cancellationToken)
        {
            var message = new Message()
            {
                UserId = request.userId,
                ConversationId = request.createMessageDto.ConversationId,
                Content = request.createMessageDto.Content,
                Timestamp = DateTime.UtcNow
            };

            var createdMessage = await _unitOfWork.MessageRepository.AddAsync(message);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Message successfully created!");

            return _mapper.Map<ResponseMessageDto>(createdMessage);
        }
    }
}
