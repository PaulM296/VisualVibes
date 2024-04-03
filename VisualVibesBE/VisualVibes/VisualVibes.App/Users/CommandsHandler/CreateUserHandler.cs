using MediatR;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.App.Users.Responses;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.CommandsHandler
{
    public class CreateUserHandler : IRequestHandler<CreateUser, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserHandler(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }
        public Task<UserDto> Handle(CreateUser request, CancellationToken cancellationToken)
        {
            var user = new User() { Id = Guid.NewGuid(), Username = request.Username, Password = request.Password,
            UserProfile = request.UserProfile, UserFeed = request.UserFeed, Followers = request.Followers, Following = request.Following };
            var createdUser = _userRepository.Add(user);
            return Task.FromResult(UserDto.FromUser(createdUser));
        }
    }
}
