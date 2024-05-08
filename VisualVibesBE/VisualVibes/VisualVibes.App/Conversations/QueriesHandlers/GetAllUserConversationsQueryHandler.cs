using MediatR;
using VisualVibes.App.Conversations.Queries;
using VisualVibes.App.DTOs.ConversationDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Conversations.QueriesHandlers
{
    public class GetAllUserConversationsQueryHandler : IRequestHandler<GetAllUserConversationsQuery, ICollection<ResponseConversationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public GetAllUserConversationsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ICollection<ResponseConversationDto>> Handle(GetAllUserConversationsQuery request, CancellationToken cancellationToken)
        {
            var conversations = await _unitOfWork.ConversationRepository.GetAllByUserIdAsync(request.UserId);

            if (conversations.Count == 0)
            {
                throw new ConversationNotFoundException($"Could not get the conversations for UserId {request.UserId}, because it doesn't have any yet!");
            }

            var conversationDtos = conversations.Select(ResponseConversationDto.FromConversation).ToList();

            return conversationDtos;
        }
    }
}
