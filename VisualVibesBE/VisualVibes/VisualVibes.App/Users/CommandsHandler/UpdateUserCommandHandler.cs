using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;

namespace VisualVibes.App.Users.CommandsHandler
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ResponseUserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdateUserCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdateUserCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseUserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _unitOfWork.UserRepository.GetUserByIdAsync(request.userId);

            if (user == null)
            {
                throw new UserNotFoundException($"The user with ID {request.userId} doesn't exist and it could not be updated!");
            }

            user.UserName = request.updateUserDto.UserName;
            user.Email = request.updateUserDto.Email;
            user.Role = request.updateUserDto.Role;
            user.UserProfile.Bio = request.updateUserDto.Bio;
            user.UserProfile.ProfilePicture = request.updateUserDto.ProfilePicture;
            user.UserProfile.FirstName = request.updateUserDto.FirstName;
            user.UserProfile.LastName = request.updateUserDto.LastName;

            var updatedUser = await _unitOfWork.UserRepository.UpdateUserAsync(user);

            await _unitOfWork.SaveAsync();

            _logger.LogInformation("User has been successfully updated!");

            return _mapper.Map<ResponseUserDto>(updatedUser);
        }
    }
}
