using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Comments.Queries;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Reactions.Queries;
using VisualVibes.App.Reactions.QueriesHandler;

namespace VisualVibes.App.Comments.QueriesHandler
{
    public class GetTotalCommentNumberForPostQueryHandler : IRequestHandler<GetTotalCommentNumberForPostQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetTotalCommentNumberForPostQueryHandler> _logger;

        public GetTotalCommentNumberForPostQueryHandler(IUnitOfWork unitOfWork, ILogger<GetTotalCommentNumberForPostQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<int> Handle(GetTotalCommentNumberForPostQuery request, CancellationToken cancellationToken)
        {
            var totalCommentsNumber = await _unitOfWork.CommentRepository.GetPostTotalCommentNumber(request.postId);

            if (totalCommentsNumber == 0)
            {
                throw new CommentsNotFoundException($"Could not get the comments from PostId {request.postId}, because it doesn't have any yet!");
            }

            _logger.LogInformation("All post comments successfully retrieved!");

            return totalCommentsNumber;
        }
    }
}
