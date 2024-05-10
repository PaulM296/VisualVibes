using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.ReactionDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Queries;

namespace VisualVibes.App.Reactions.QueriesHandler
{
    public class GetAllPostReactionsQueryHandler : IRequestHandler<GetAllPostReactionsQuery, ICollection<ResponseReactionDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllPostReactionsQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllPostReactionsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllPostReactionsQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ICollection<ResponseReactionDto>> Handle(GetAllPostReactionsQuery request, CancellationToken cancellationToken)
        {
            var reactions = await _unitOfWork.ReactionRepository.GetAllAsync(request.PostId);

            if (reactions.Count == 0)
            {
                throw new ReactionNotFoundException($"Could not get the reactions from PostId {request.PostId}, because it doesn't have any yet!");
            }

            var reactionDtos = _mapper.Map<ICollection<ResponseReactionDto>>(reactions);

            _logger.LogInformation("All post reactions successfully retrieved!");

            return reactionDtos;
        }
    }
}
