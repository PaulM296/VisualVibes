using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.MessageDtos;
using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.Queries;

namespace VisualVibes.App.Messages.QueriesHandler
{
    public class GetAllConversationMessagesQueryHandler : IRequestHandler<GetAllConversationMessagesQuery, PaginationResponseDto<ResponseMessageDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllConversationMessagesQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllConversationMessagesQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllConversationMessagesQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<PaginationResponseDto<ResponseMessageDto>> Handle(GetAllConversationMessagesQuery request, CancellationToken cancellationToken)
        {
            var messages = await _unitOfWork.MessageRepository
                .GetAllPagedConversationMessagesAsync(request.ConversationId, request.paginationRequestDto.PageIndex,
                request.paginationRequestDto.PageSize);

            if (messages.Items.Count == 0)
            {
                throw new MessageNotFoundException($"Could not get the messages from ConversationId {request.ConversationId}, because it doesn't have any yet!");
            }

            var messagesDtos = new PaginationResponseDto<ResponseMessageDto>(
                items: _mapper.Map<List<ResponseMessageDto>>(messages.Items),
                pageIndex: messages.PageIndex,
                totalPages: messages.TotalPages);

            _logger.LogInformation("Conversation messages successfully retrieved!");

            return messagesDtos;
        }
    }
}
