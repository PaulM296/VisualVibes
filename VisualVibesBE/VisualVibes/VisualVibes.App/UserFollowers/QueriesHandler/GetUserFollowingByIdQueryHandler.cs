using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.UserFollowerDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserFollowers.Queries;

namespace VisualVibes.App.UserFollowers.QueriesHandler
{
    public class GetUserFollowingByIdQueryHandler : IRequestHandler<GetUserFollowingByIdQuery, IEnumerable<UserFollowerDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetUserFollowersByIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetUserFollowingByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserFollowersByIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserFollowerDto>> Handle(GetUserFollowingByIdQuery request, CancellationToken cancellationToken)
        {
            var following = await _unitOfWork.UserFollowerRepository.GetFollowingByUserIdAsync(request.UserId);

            var followingDtos = following.Select(f => new UserFollowerDto
            {
                FollowerId = f.FollowerId,
                FollowingId = f.FollowingId,
                UserName = f.Following.UserName,
                FirstName = f.Following.UserProfile.FirstName,
                LastName = f.Following.UserProfile.LastName,
                ImageId = f.Following.UserProfile.ImageId
            });

            _logger.LogInformation("Following successfully retrieved!");

            return followingDtos;
        }
    }
}
