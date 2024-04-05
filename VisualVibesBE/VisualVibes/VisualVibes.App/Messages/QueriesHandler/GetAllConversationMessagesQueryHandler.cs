using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Queries;

namespace VisualVibes.App.Messages.QueriesHandler
{
    public class GetAllConversationMessagesQueryHandler : IRequestHandler<GetAllConversationMessagesQuery, ICollection<MessageDto>>
    {
        private readonly IMessageRepository _messageRepository;
        public GetAllConversationMessagesQueryHandler(IMessageRepository messageRepository) 
        {
            _messageRepository = messageRepository;
        }
        public async Task<ICollection<MessageDto>> Handle(GetAllConversationMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _messageRepository.GetAllAsync(request.ConversationId);

            if (messages.Count == 0)
            {
                throw new ApplicationException("Messages not found");
            }

            var messagesDtos = new List<MessageDto>();
            foreach (var message in messages)
            {
                messagesDtos.Add(MessageDto.FromMessage(message));
            }

            return messagesDtos;
        }
    }
}
