using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.CommandsHandler
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandHandler(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User() 
            { 
                Id = request.UserDto.Id,
                Username = request.UserDto.Username, 
                Password = request.UserDto.Password,
                Followers = new List<User>(),
                Following = new List<User>()
            };
            var createdUser = await _userRepository.AddAsync(user);
            return UserDto.FromUser(createdUser);
        }
    }
}
