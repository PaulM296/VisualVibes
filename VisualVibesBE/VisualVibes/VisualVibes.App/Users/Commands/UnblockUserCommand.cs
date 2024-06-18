using MediatR;

namespace VisualVibes.App.Users.Commands
{
    public record UnblockUserCommand(string userId) : IRequest<Unit>;
}
