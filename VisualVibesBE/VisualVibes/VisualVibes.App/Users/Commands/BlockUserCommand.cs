using MediatR;

namespace VisualVibes.App.Users.Commands
{
    public record BlockUserCommand(string userId) : IRequest<Unit>;
}
