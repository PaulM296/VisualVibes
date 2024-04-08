using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserProfiles.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.UserProfiles.CommandsHandler
{
    public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, UserProfileDto>
    {
        private readonly IUserProfileRepository _userProfileRepository;
        public CreateUserProfileCommandHandler(IUserProfileRepository userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;
        }
        public async Task<UserProfileDto> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userProfile = new UserProfile
            {
                Id = Guid.NewGuid(),
                ProfilePicture = request.UserProfileDto.ProfilePicture,
                DateOfBirth = request.UserProfileDto.DateOfBirth,
                FirstName = request.UserProfileDto.FirstName,
                LastName = request.UserProfileDto.LastName,
                Email = request.UserProfileDto.Email,
                Bio = request.UserProfileDto.Bio,
            };
            var userProfileCreated = await _userProfileRepository.AddAsync(userProfile);
            return UserProfileDto.FromUserProfile(userProfileCreated);
        }
    }
}
