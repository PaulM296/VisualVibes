﻿using MediatR;
using VisualVibes.App.Conversations.Commands;
using VisualVibes.App.DTOs.ConversationDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Conversations.CommandsHandler
{
    public class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, ResponseConversationDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateConversationCommandHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseConversationDto> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
        {
            var conversation = new Conversation()
            {
                Id = Guid.NewGuid(),
                FirstParticipantId = request.createConversationDto.FirstParticipantId,
                SecondParticipantId = request.createConversationDto.SecondParticipantId,
            };

            var createdConversation = await _unitOfWork.ConversationRepository.AddAsync(conversation);
            await _unitOfWork.SaveAsync();

            return ResponseConversationDto.FromConversation(conversation);
        }
    }
}
