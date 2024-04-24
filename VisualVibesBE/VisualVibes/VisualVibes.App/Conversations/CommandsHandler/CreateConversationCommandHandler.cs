using MediatR;
using VisualVibes.App.Conversations.Commands;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Conversations.CommandsHandler
{
    public class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, ConversationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateConversationCommandHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ConversationDto> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            var conversation = new Conversation()
            {
                Id = request.ConversationDto.Id,
                FirstParticipantId = request.ConversationDto.FirstParticipantId,
                SecondParticipantId = request.ConversationDto.SecondParticipantId,
            };

            var createdConversation = await _unitOfWork.ConversationRepository.AddAsync(conversation);
            await _unitOfWork.SaveAsync();

            return ConversationDto.FromConversation(conversation);
        }
    }
}
