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
            var comment = new Comment()
            {
                Id = request.Id
            };
            await _commentRepository.RemoveAsync(comment);
            return Unit.Value;
        }
    }
}
