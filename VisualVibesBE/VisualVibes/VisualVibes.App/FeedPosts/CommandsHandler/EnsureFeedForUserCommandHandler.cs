using MediatR;
using VisualVibes.App.FeedPosts.Commands;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.FeedPosts.CommandsHandler
{
    internal class EnsureFeedForUserCommandHandler : IRequestHandler<EnsureFeedForUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public EnsureFeedForUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(EnsureFeedForUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.FeedPostRepository.EnsureFeedForUserAsync(request.userId);
            return Unit.Value;
        }
    }
}
