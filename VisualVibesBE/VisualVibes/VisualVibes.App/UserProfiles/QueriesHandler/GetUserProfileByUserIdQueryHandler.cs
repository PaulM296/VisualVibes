using MediatR;
using VisualVibes.App.DTOs.UserProfileDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserProfiles.Queries;

namespace VisualVibes.App.UserProfiles.QueriesHandler
{
    public class GetUserProfileByUserIdQueryHandler : IRequestHandler<GetUserProfileByUserIdQuery, ResponseUserProfileDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetUserProfileByUserIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseUserProfileDto> Handle(GetUserProfileByUserIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserProfileRepository.GetUserWithProfileByIdAsync(request.userId);

            if (user == null || user.UserProfile == null)
            {
                throw new UserProfileNotFoundException($"The user profile with userId: {request.userId} has not been found!");
            }

            return ResponseUserProfileDto.FromUserProfile(user.UserProfile);
        }
    }
}
