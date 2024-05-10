using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;

namespace VisualVibes.App.Users.CommandsHandler
{
    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveUserCommandHandler> _logger;

        public RemoveUserCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var userToRemove = await _unitOfWork.UserRepository.GetByIdAsync(request.Id);


            if (userToRemove == null)
            {
                throw new UserNotFoundException($"The user with ID {request.Id} doesn't exist and it could not be removed!");
            }

            var followers = await _unitOfWork.UserFollowerRepository.GetFollowersByUserIdAsync(request.Id);
            var followings = await _unitOfWork.UserFollowerRepository.GetFollowingByUserIdAsync(request.Id);

            foreach (var follower in followers)
            {
                await _unitOfWork.UserFollowerRepository.RemoveFollowerAsync(follower.FollowerId, follower.FollowingId);
            }

            foreach (var following in followings)
            {
                await _unitOfWork.UserFollowerRepository.RemoveFollowerAsync(following.FollowerId, following.FollowingId);
            }

            var conversations = await _unitOfWork.ConversationRepository.GetAllByUserIdAsync(request.Id);
            foreach (var conversation in conversations)
            {
                await _unitOfWork.ConversationRepository.RemoveAsync(conversation);
            }

            var feed = await _unitOfWork.FeedRepository.GetByUserIdAsync(request.Id);
            if (feed != null)
            {
                var feedPosts = await _unitOfWork.FeedPostRepository.GetByFeedIdAsync(feed.Id);
                foreach (var feedPost in feedPosts)
                {
                    await _unitOfWork.FeedPostRepository.RemoveAsync(feedPost);
                }

                // Now remove the feed
                await _unitOfWork.FeedRepository.RemoveAsync(feed);
            }

            await _unitOfWork.UserRepository.RemoveAsync(userToRemove);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("User succesfully !");

            return Unit.Value;
        }
    }
}
