using MediatR;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Comments.CommandsHandler
{
    public class RemoveCommentCommandHandler : IRequestHandler<RemoveCommentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemoveCommentCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Unit> Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
        {
            var commentToRemove = await _unitOfWork.CommentRepository.GetByIdAsync(request.Id);

            if (commentToRemove == null)
            {
                Console.WriteLine($"Failed to find comment with ID {request.Id} for removal.");
                throw new Exception($"Comment with ID {request.Id} not found.");
            }
            else
            {
                Console.WriteLine($"Removing comment with ID {request.Id}.");
            }

            await _unitOfWork.CommentRepository.RemoveAsync(commentToRemove);
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
