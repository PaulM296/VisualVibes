using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserProfiles.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.UserProfiles.CommandsHandler
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UserProfileDto>
    {
        private readonly IUserProfileRepository _userProfileRepository;

        public UpdateUserProfileCommandHandler(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }
        public async Task<UserProfileDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userProfile = new UserProfile()
            {
                Id = Guid.NewGuid(),
                User = request.UserProfileDto.User,
                ProfilePicture = request.UserProfileDto.ProfilePicture,
                DateOfBirth = request.UserProfileDto.DateOfBirth,
                FirstName = request.UserProfileDto.FirstName,
                LastName = request.UserProfileDto.LastName,
                Email = request.UserProfileDto.Email,
                Bio = request.UserProfileDto.Bio
            };
            var updatedUserProfile = await _userProfileRepository.UpdateAsync(userProfile);
            return UserProfileDto.FromUserProfile(updatedUserProfile);
        }
    }
}
