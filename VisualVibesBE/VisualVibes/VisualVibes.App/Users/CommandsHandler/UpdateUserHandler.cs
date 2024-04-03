using MediatR;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.App.Users.Responses;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.CommandsHandler
{
    public class UpdateUserHandler : IRequestHandler<UpdateUser, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserHandler(IUserRepository userRepository) 
        {
            _userRepository = userRepository;
        }

        public Task<UserDto> Handle(UpdateUser request, CancellationToken cancellationToken)
        {
            var user = new User() { Id = request.Id, Username = request.Username, Password = request.Password,
                UserFeed = request.UserFeed, Followers = request.Followers, Following = request.Following };
            var updatedUser = _userRepository.Update(user);
            return Task.FromResult(UserDto.FromUser(updatedUser));
        }
    }
}
