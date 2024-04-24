using MediatR;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Posts.CommandsHandler
{
    public class RemovePostCommandHandler : IRequestHandler<RemovePostCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public RemovePostCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RemovePostCommand request, CancellationToken cancellationToken)
        {
            var postToRemove = await _unitOfWork.PostRepository.GetByIdAsync(request.Id);

            if (postToRemove == null)
            {
                throw new Exception($"Post with ID {request.Id} not found.");
            }

            await _unitOfWork.PostRepository.RemoveAsync(postToRemove);
            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
