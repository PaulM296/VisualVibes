using MediatR;
using VisualVibes.App.DTOs.UserProfileDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserProfiles.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.UserProfiles.CommandsHandler
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, ResponseUserProfileDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUserProfileCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
            
        public async Task<ResponseUserProfileDto> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var getUserProfile = await _unitOfWork.UserProfileRepository.GetByIdAsync(request.id);

            if (getUserProfile == null)
            {
                throw new UserProfileNotFoundException($"The userProfile with Id {request.id} doesn't exist and it could not be updated!");
            }

            getUserProfile.ProfilePicture = request.updateUserProfileDto.ProfilePicture;
            getUserProfile.DateOfBirth = request.updateUserProfileDto.DateOfBirth;
            getUserProfile.FirstName = request.updateUserProfileDto.FirstName;
            getUserProfile.LastName = request.updateUserProfileDto.LastName;
            getUserProfile.Email = request.updateUserProfileDto.Email;
            getUserProfile.Bio = request.updateUserProfileDto.Bio;

            var updatedUserProfile = await _unitOfWork.UserProfileRepository.UpdateAsync(getUserProfile);
            await _unitOfWork.SaveAsync();

            return ResponseUserProfileDto.FromUserProfile(updatedUserProfile);
        }
    }
}
