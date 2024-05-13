using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.DTOs;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Comments.CommandsHandler
{
    public class RemoveCommentCommandHandler : IRequestHandler<RemoveCommentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RemoveCommentCommandHandler> _logger;

        public RemoveCommentCommandHandler(IUnitOfWork unitOfWork, ILogger<RemoveCommentCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<Unit> Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
        {
            var commentToRemove = await _unitOfWork.CommentRepository.GetByIdAsync(request.Id);

            if (commentToRemove == null)
            {
                throw new CommentsNotFoundException($"The comment with ID {request.Id} doesn't exist and it could not be removed!");
            }

            await _unitOfWork.CommentRepository.RemoveAsync(commentToRemove);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Comment successfully removed!");

            return Unit.Value;
        }
    }
}
