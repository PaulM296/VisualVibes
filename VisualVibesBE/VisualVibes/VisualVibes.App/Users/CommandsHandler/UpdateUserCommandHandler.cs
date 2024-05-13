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
            var user = await _unitOfWork.UserRepository.GetByIdAsync(request.userId);

            if (user == null)
            {
                throw new UserNotFoundException($"The user with ID {request.userId} doesn't exist and it could not be updated!");
            }

            user.Username = request.updateUserDto.Username;

            var updatedUser = await _unitOfWork.UserRepository.UpdateAsync(user);

            await _unitOfWork.SaveAsync();

            _logger.LogInformation("User has been successfully updated!");

            return _mapper.Map<ResponseUserDto>(updatedUser);
        }
    }
}
