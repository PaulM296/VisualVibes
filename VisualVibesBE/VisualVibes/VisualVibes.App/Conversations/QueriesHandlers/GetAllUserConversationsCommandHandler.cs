using MediatR;
using VisualVibes.App.Conversations.Queries;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Conversations.QueriesHandlers
{
    public class GetAllUserConversationsCommandHandler : IRequestHandler<GetAllUserConversationsCommand, ICollection<ConversationDto>>
    {
        private readonly IConversationRepository _conversationRepository;
        public GetAllUserConversationsCommandHandler(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }
        public async Task<ICollection<ConversationDto>> Handle(GetAllUserConversationsCommand request, CancellationToken cancellationToken)
        {
            var conversations = await _conversationRepository.GetAllAsync();

            if (conversations.Count == 0)
            {
                throw new ApplicationException("Conversations not found");
            }

            var conversationDtos = new List<ConversationDto>();
            foreach (var conversation in conversations)
            {
                conversationDtos.Add(ConversationDto.FromConversation(conversation));
            }

            return conversationDtos;
        }
    }
}
