using MediatR;
using VisualVibes.App.DTOs.UserFollowerDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserFollowers.Queries;

namespace VisualVibes.App.UserFollowers.QueriesHandler
{
    public class GetUserFollowingByIdQueryHandler : IRequestHandler<GetUserFollowingByIdQuery, IEnumerable<UserFollowerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserFollowingByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserFollowerDto>> Handle(GetUserFollowingByIdQuery request, CancellationToken cancellationToken)
        {
            var following = await _unitOfWork.UserFollowerRepository.GetFollowingByUserIdAsync(request.UserId);

            var followingDtos = following.Select(UserFollowerDto.FromUserFollower).ToList();

            return followingDtos;
        }
    }
}
