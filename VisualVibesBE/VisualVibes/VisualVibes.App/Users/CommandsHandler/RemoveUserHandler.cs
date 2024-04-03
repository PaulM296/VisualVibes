using MediatR;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.App.Users.Responses;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.CommandsHandler
{
    public class RemoveUserHandler : IRequestHandler<RemoveUser, Unit>
    {
        private readonly IUserRepository _userRepository;

        public RemoveUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<Unit> Handle(RemoveUser request, CancellationToken cancellationToken)
        {
            var user = new User { Id = request.Id };
            _userRepository.Remove(user);
            return Task.FromResult(Unit.Value);
        }
    }
}
