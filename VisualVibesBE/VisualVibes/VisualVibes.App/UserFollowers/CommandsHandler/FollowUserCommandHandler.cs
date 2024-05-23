using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserFollowers.Commands;
using VisualVibes.Domain.Models.BaseEntity;
using VisualVibes.Domain.Models;

namespace VisualVibes.App.UserFollowers.CommandsHandler
{
    public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FollowUserCommandHandler> _logger;

        public FollowUserCommandHandler(IUnitOfWork unitOfWork, ILogger<FollowUserCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserFollowerRepository.AddFollowerAsync(request.FollowerId, request.FollowingId);

            await AddPostsToFeedAsync(request.FollowerId, request.FollowingId);

            await _unitOfWork.SaveAsync();

            _logger.LogInformation("User successfully followed!");

            return Unit.Value;
        }

        private async Task AddPostsToFeedAsync(string followerId, string followingId)
        {
            var posts = await _unitOfWork.PostRepository.GetPostsByUserIdAsync(followingId);

            foreach (var post in posts)
            {
                var feed = await _unitOfWork.FeedRepository.GetByUserIdAsync(followerId);
                if (feed == null)
                {
                    feed = new Feed { UserID = followerId };
                    await _unitOfWork.FeedRepository.AddAsync(feed);
                    await _unitOfWork.SaveAsync();
                }

                var feedPostExists = await _unitOfWork.FeedPostRepository.ExistsAsync(feed.Id, post.Id);
                if (!feedPostExists)
                {
                    await _unitOfWork.FeedPostRepository.AddAsync(new FeedPost
                    {
                        FeedId = feed.Id,
                        PostId = post.Id,
                    });
                }
            }

            await _unitOfWork.SaveAsync();
        }
    }
}
