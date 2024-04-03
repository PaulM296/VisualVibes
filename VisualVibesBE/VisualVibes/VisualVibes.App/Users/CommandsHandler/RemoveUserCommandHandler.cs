using MediatR;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.CommandsHandler
{
    public class RemoveUserCommandHandler : IRequestHandler<RemoveUserCommand, Unit>
    {
        private readonly IUserRepository _userRepository;

        public RemoveUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Unit> Handle(RemoveUserCommand request, CancellationToken cancellationToken)
        {
            var user = new User
            {
                Id = request.Id
            };
            await _userRepository.RemoveAsync(user);
            return Unit.Value;
        }
    }
}
