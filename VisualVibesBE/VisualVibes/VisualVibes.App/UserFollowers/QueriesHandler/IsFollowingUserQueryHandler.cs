using MediatR;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserFollowers.Queries;

namespace VisualVibes.App.UserFollowers.QueriesHandler
{
    public class IsFollowingUserQueryHandler : IRequestHandler<IsFollowingUserQuery, bool>
    {
        private readonly IUnitOfWork _unitOfWork;

        public IsFollowingUserQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(IsFollowingUserQuery request, CancellationToken cancellationToken)
        {
            var isFollowing = await _unitOfWork.UserFollowerRepository.IsFollowingAsync(request.followerId, request.followingId);

            return isFollowing;
        }
    }
}
