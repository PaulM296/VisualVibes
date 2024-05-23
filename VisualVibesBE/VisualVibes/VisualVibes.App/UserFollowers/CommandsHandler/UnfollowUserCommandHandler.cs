using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserFollowers.Commands;

namespace VisualVibes.App.UserFollowers.CommandsHandler
{
    public class UnfollowUserCommandHandler : IRequestHandler<UnfollowUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UnfollowUserCommandHandler> _logger;

        public UnfollowUserCommandHandler(IUnitOfWork unitOfWork, ILogger<UnfollowUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserFollowerRepository.RemoveFollowerAsync(request.FollowerId, request.FollowingId);

            var feed = await _unitOfWork.FeedRepository.GetByUserIdAsync(request.FollowerId);

            if(feed == null)
            {
                throw new FeedNotFoundException($"Feed for user {request.FollowerId} not found!");
            }

            var postsToRemove = await _unitOfWork.UserFollowerRepository.GetFeedPostsByUserIdAsync(feed.Id, request.FollowingId);

            foreach (var feedPost in postsToRemove)
            {
                await _unitOfWork.FeedPostRepository.RemoveAsync(feedPost);
            }

            await _unitOfWork.SaveAsync();

            _logger.LogInformation("User successfully unfollowed!");

            return Unit.Value;
        }
    }
}
