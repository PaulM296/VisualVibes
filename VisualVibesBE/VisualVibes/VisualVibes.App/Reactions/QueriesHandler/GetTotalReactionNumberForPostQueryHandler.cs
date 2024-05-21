using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Queries;

namespace VisualVibes.App.Reactions.QueriesHandler
{
    public class GetTotalReactionNumberForPostQueryHandler : IRequestHandler<GetTotalReactionNumberForPostQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetTotalReactionNumberForPostQueryHandler> _logger;

        public GetTotalReactionNumberForPostQueryHandler(IUnitOfWork unitOfWork, ILogger<GetTotalReactionNumberForPostQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<int> Handle(GetTotalReactionNumberForPostQuery request, CancellationToken cancellationToken)
        {
            var totalReactionsNumber = await _unitOfWork.ReactionRepository.GetPostTotalReactionNumber(request.postId);

            if (totalReactionsNumber == 0)
            {
                throw new ReactionNotFoundException($"Could not get the reactions from PostId {request.postId}, because it doesn't have any yet!");
            }

            _logger.LogInformation("All post reactions successfully retrieved!");

            return totalReactionsNumber;
        }
    }
}
