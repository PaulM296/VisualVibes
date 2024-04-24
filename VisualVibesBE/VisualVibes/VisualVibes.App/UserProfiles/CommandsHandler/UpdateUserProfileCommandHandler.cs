using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserProfiles.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.UserProfiles.CommandsHandler
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, UserProfileDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserProfileCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
            
        public async Task<UserProfileDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var getUserProfile = await _unitOfWork.UserProfileRepository.GetByIdAsync(request.UserProfileDto.Id);

            if (getUserProfile == null)
            {
                throw new Exception("UserProfile not found");
            }

            getUserProfile.ProfilePicture = request.UserProfileDto.ProfilePicture;
            getUserProfile.DateOfBirth = request.UserProfileDto.DateOfBirth;
            getUserProfile.FirstName = request.UserProfileDto.FirstName;
            getUserProfile.LastName = request.UserProfileDto.LastName;
            getUserProfile.Email = request.UserProfileDto.Email;
            getUserProfile.Bio = request.UserProfileDto.Bio;

            var updatedUserProfile = await _unitOfWork.UserProfileRepository.UpdateAsync(getUserProfile);
            await _unitOfWork.SaveAsync();

            return UserProfileDto.FromUserProfile(updatedUserProfile);
        }
    }
}
