using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Comments.CommandsHandler
{
    public class ModerateCommentCommandHandler : IRequestHandler<ModerateCommentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ModerateCommentCommandHandler> _logger;

        public ModerateCommentCommandHandler(IUnitOfWork unitOfWork, ILogger<ModerateCommentCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Unit> Handle(ModerateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _unitOfWork.CommentRepository.GetByIdAsync(request.commentId);

            if(comment == null)
            {
                throw new CommentsNotFoundException($"Comment with ID {request.commentId} has not been found!");
            }

            comment.isModerated = true;
            await _unitOfWork.CommentRepository.UpdateAsync(comment);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation($"Comment with ID {request.commentId} has been moderated.");

            return Unit.Value;
        }
    }
}
