using MediatR;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserFollowers.Commands;

namespace VisualVibes.App.UserFollowers.CommandsHandler
{
    public class FollowUserCommandHandler : IRequestHandler<FollowUserCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FollowUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(FollowUserCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.UserFollowerRepository.AddFollowerAsync(request.FollowerId, request.FollowingId);

            await _unitOfWork.SaveAsync();

            return Unit.Value;
        }
    }
}
