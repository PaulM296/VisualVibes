using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserProfiles.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.UserProfiles.CommandsHandler
{
    public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, UserProfileDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserProfileCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<UserProfileDto> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _unitOfWork.UserRepository.GetByIdAsync(request.UserProfileDto.UserId);
            if (userExists == null)
            {
                throw new UserNotFoundException($"The user with ID {request.UserProfileDto.UserId} doesn't exist!");    
            }

            var userProfile = new UserProfile
            {
                Id = request.UserProfileDto.Id,
                UserId = request.UserProfileDto.UserId,
                ProfilePicture = request.UserProfileDto.ProfilePicture,
                DateOfBirth = request.UserProfileDto.DateOfBirth,
                FirstName = request.UserProfileDto.FirstName,
                LastName = request.UserProfileDto.LastName,
                Email = request.UserProfileDto.Email,
                Bio = request.UserProfileDto.Bio,
            };

            await _unitOfWork.UserProfileRepository.AddAsync(userProfile);
            await _unitOfWork.SaveAsync();

            return UserProfileDto.FromUserProfile(userProfile);
        }
    }
}
