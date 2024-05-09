using MediatR;
using VisualVibes.App.DTOs.UserProfileDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserProfiles.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.UserProfiles.CommandsHandler
{
    public class CreateUserProfileCommandHandler : IRequestHandler<CreateUserProfileCommand, ResponseUserProfileDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserProfileCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseUserProfileDto> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            var userExists = await _unitOfWork.UserRepository.GetByIdAsync(request.createUserProfileDto.UserId);
            if (userExists == null)
            {
                throw new UserNotFoundException($"The user with ID {request.createUserProfileDto.UserId} doesn't exist!");    
            }

            var userProfile = new UserProfile
            {
                UserId = request.createUserProfileDto.UserId,
                ProfilePicture = request.createUserProfileDto.ProfilePicture,
                DateOfBirth = request.createUserProfileDto.DateOfBirth,
                FirstName = request.createUserProfileDto.FirstName,
                LastName = request.createUserProfileDto.LastName,
                Email = request.createUserProfileDto.Email,
                Bio = request.createUserProfileDto.Bio,
            };

            await _unitOfWork.UserProfileRepository.AddAsync(userProfile);
            await _unitOfWork.SaveAsync();

            return ResponseUserProfileDto.FromUserProfile(userProfile);
        }
    }
}
