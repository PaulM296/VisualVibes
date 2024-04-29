using MediatR;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserFollowers.Commands;

namespace VisualVibes.App.UserFollowers.CommandsHandler
{
    public class UnfollowUserCommandHandler : IRequestHandler<UnfollowUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnfollowUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UnfollowUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserFollowerRepository.RemoveFollowerAsync(request.FollowerId, request.FollowingId);
            return Unit.Value;
        }
    }
}
