using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;

namespace VisualVibes.App.Posts.CommandsHandler
{
    public class ModeratePostCommandHandler : IRequestHandler<ModeratePostCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ModeratePostCommandHandler> _logger;

        public ModeratePostCommandHandler(IUnitOfWork unitOfWork, ILogger<ModeratePostCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(ModeratePostCommand request, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(request.postId);

            if(post == null)
            {
                throw new PostNotFoundException($"Post with ID {request.postId} has not been found!");
            }

            post.isModerated = true;
            await _unitOfWork.PostRepository.UpdateAsync(post);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation($"Post with ID {request.postId} has been moderated!");

            return Unit.Value;
        }
    }
}
