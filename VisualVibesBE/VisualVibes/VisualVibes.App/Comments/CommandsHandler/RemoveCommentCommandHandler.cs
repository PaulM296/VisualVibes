using MediatR;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Comments.CommandsHandler
{
    public class RemoveCommentCommandHandler : IRequestHandler<RemoveCommentCommand, Unit>
    {
        private readonly ICommentRepository _commentRepository;

        public RemoveCommentCommandHandler(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }
        public async Task<Unit> Handle(RemoveCommentCommand request, CancellationToken cancellationToken)
        {
            var commentToRemove = await _commentRepository.GetByIdAsync(request.Id);
            if(commentToRemove == null)
            {
                throw new Exception($"Comment with ID {request.Id} not found.");
            };
            await _commentRepository.RemoveAsync(commentToRemove);
            return Unit.Value;
        }
    }
}
