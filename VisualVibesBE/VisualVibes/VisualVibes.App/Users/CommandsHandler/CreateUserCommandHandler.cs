using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.CommandsHandler
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ResponseUserDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateUserCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ResponseUserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Username = request.createUserDto.Username,
                Password = request.createUserDto.Password,
            };

            var createdUser = await _unitOfWork.UserRepository.AddAsync(user);
            // Ensure a feed is created for the new user
            var feed = new Feed 
            { 
                UserID = createdUser.Id 
            };
            await _unitOfWork.FeedRepository.AddAsync(feed);
            await _unitOfWork.FeedPostRepository.EnsureFeedForUserAsync(user.Id);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("New user successfully added!");

            return _mapper.Map<ResponseUserDto>(createdUser);
        }
    }
}
