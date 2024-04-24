using MediatR;
using VisualVibes.App.Conversations.Queries;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Conversations.QueriesHandlers
{
    public class GetAllUserConversationsQueryHandler : IRequestHandler<GetAllUserConversationsQuery, ICollection<ConversationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllUserConversationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ICollection<ConversationDto>> Handle(GetAllUserConversationsQuery request, CancellationToken cancellationToken)
        {
            var conversations = await _unitOfWork.ConversationRepository.GetAllByUserIdAsync(request.UserId);

            if (conversations.Count == 0)
            {
                throw new ApplicationException("Conversations not found");
            }

            var conversationDtos = conversations.Select(ConversationDto.FromConversation).ToList();

            return conversationDtos;
        }
    }
}
