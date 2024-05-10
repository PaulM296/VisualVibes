using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.MessageDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Queries;

namespace VisualVibes.App.Messages.QueriesHandler
{
    public class GetAllConversationMessagesQueryHandler : IRequestHandler<GetAllConversationMessagesQuery, ICollection<ResponseMessageDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllConversationMessagesQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllConversationMessagesQueryHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ICollection<ResponseMessageDto>> Handle(GetAllConversationMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _unitOfWork.MessageRepository.GetAllAsync(request.ConversationId);

            if (messages.Count == 0)
            {
                throw new MessageNotFoundException($"Could not get the messages from ConversationId {request.ConversationId}, because it doesn't have any yet!");
            }

            var messagesDtos = _mapper.Map<ICollection<ResponseMessageDto>>(messages);

            _logger.LogInformation("Conversation messages successfully retrieved!");

            return messagesDtos;
        }
    }
}
