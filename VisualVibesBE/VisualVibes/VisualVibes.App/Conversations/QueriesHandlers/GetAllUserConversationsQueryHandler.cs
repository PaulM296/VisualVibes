using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Conversations.Queries;
using VisualVibes.App.DTOs.ConversationDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Conversations.QueriesHandlers
{
    public class GetAllUserConversationsQueryHandler : IRequestHandler<GetAllUserConversationsQuery, ICollection<ResponseConversationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllUserConversationsQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllUserConversationsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllUserConversationsQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ICollection<ResponseConversationDto>> Handle(GetAllUserConversationsQuery request, CancellationToken cancellationToken)
        {
            var conversations = await _unitOfWork.ConversationRepository.GetAllByUserIdAsync(request.UserId);

            if (conversations.Count == 0)
            {
                throw new ConversationNotFoundException($"Could not get the conversations for UserId {request.UserId}, because it doesn't have any yet!");
            }

            var conversationDtos = _mapper.Map<ICollection<ResponseConversationDto>>(conversations);

            _logger.LogInformation("All user conversations successfully retrieved!");

            return conversationDtos;
        }
    }
}
