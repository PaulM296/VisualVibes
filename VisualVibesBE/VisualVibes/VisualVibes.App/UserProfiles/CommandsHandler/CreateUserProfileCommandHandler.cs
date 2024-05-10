using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<CreateUserProfileCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateUserProfileCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateUserProfileCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
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

            _logger.LogInformation("New UserProfile has been successfully created!");

            return _mapper.Map<ResponseUserProfileDto>(userProfile);
        }
    }
}
