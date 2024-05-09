using MediatR;
using VisualVibes.App.DTOs.UserFollowerDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserFollowers.Queries;

namespace VisualVibes.App.UserFollowers.QueriesHandler
{
    public class GetUserFollowersByIdQueryHandler : IRequestHandler<GetUserFollowersByIdQuery, IEnumerable<UserFollowerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserFollowersByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<UserFollowerDto>> Handle(GetUserFollowersByIdQuery request, CancellationToken cancellationToken)
        {
            var followers = await _unitOfWork.UserFollowerRepository.GetFollowersByUserIdAsync(request.UserId);

            var followerDtos = followers.Select(UserFollowerDto.FromUserFollower).ToList();

            return followerDtos;
        }
    }
}
