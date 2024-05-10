using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;

namespace VisualVibes.App.Posts.CommandsHandler
{
    public class RemovePostCommandHandler : IRequestHandler<RemovePostCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemovePostCommandHandler> _logger;

        public RemovePostCommandHandler(IUnitOfWork unitOfWork, ILogger<RemovePostCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(RemovePostCommand request, CancellationToken cancellationToken)
        {
            var postToRemove = await _unitOfWork.PostRepository.GetByIdAsync(request.Id);

            if (postToRemove == null)
            {
                throw new PostNotFoundException($"The post with ID {request.Id} doesn't exist and it could not be removed!");
            }

            var relatedFeedPosts = await _unitOfWork.FeedPostRepository.GetByPostIdAsync(request.Id);
            foreach (var feedPost in relatedFeedPosts)
            {
                await _unitOfWork.FeedPostRepository.RemoveAsync(feedPost);
            }

            await _unitOfWork.PostRepository.RemoveAsync(postToRemove);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Post successfully removed!");

            return Unit.Value;
        }
    }
}
