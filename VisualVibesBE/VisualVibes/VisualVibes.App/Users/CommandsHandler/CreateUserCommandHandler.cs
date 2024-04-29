using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.CommandsHandler
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateUserCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Id = request.UserDto.Id,
                Username = request.UserDto.Username,
                Password = request.UserDto.Password,
            };

            var createdUser = await _unitOfWork.UserRepository.AddAsync(user);
            // Ensure a feed is created for the new user
            var feed = new Feed { UserID = createdUser.Id };
            await _unitOfWork.FeedRepository.AddAsync(feed);
            await _unitOfWork.SaveAsync();

            return UserDto.FromUser(createdUser);
        }
    }
}
