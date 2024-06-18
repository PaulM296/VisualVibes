using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;

namespace VisualVibes.App.Posts.CommandsHandler
{
    public class UnmoderatePostCommandHandler : IRequestHandler<UnmoderatePostCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UnmoderatePostCommandHandler> _logger;

        public UnmoderatePostCommandHandler(IUnitOfWork unitOfWork, ILogger<UnmoderatePostCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(UnmoderatePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(request.postId);

            if (post == null)
            {
                throw new PostNotFoundException($"Post with ID {request.postId} has not been found!");
            }

            post.isModerated = false;
            await _unitOfWork.PostRepository.UpdateAsync(post);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation($"Post with ID {request.postId} has been unmoderated!");

            return Unit.Value;
        }
    }
}
