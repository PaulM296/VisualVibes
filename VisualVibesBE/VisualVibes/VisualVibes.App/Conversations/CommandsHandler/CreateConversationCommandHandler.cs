using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Conversations.Commands;
using VisualVibes.App.DTOs.ConversationDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Conversations.CommandsHandler
{
    public class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, ResponseConversationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateConversationCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateConversationCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateConversationCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ResponseConversationDto> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            var conversation = new Conversation()
            {
                Id = Guid.NewGuid(),
                FirstParticipantId = request.userId,
                SecondParticipantId = request.createConversationDto.SecondParticipantId,
            };

            var createdConversation = await _unitOfWork.ConversationRepository.AddAsync(conversation);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Conversation successfully created!");

            return _mapper.Map<ResponseConversationDto>(createdConversation);
        }
    }
}
