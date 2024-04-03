using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.CommandsHandler
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandHandler(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User()
            {
                Id = request.UserDto.Id,
                Username = request.UserDto.Username,
                Password = request.UserDto.Password,
                UserFeed = request.UserDto.UserFeed,
                Followers = request.UserDto.Followers,
                Following = request.UserDto.Following
            };
            var updatedUser = await _userRepository.UpdateAsync(user);
            return UserDto.FromUser(updatedUser);
        }
    }
}
