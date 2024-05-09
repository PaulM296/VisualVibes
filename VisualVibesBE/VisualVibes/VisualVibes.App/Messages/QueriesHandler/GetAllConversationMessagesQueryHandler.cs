using MediatR;
using VisualVibes.App.DTOs.MessageDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Queries;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Messages.QueriesHandler
{
    public class GetAllConversationMessagesQueryHandler : IRequestHandler<GetAllConversationMessagesQuery, ICollection<ResponseMessageDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
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

            var messagesDtos = new List<ResponseMessageDto>();
            foreach (var message in messages)
            {
                messagesDtos.Add(ResponseMessageDto.FromMessage(message));
            }

            var messagesDto = messages.Select(ResponseMessageDto.FromMessage).ToList();

            return messagesDtos;
        }
    }
}
