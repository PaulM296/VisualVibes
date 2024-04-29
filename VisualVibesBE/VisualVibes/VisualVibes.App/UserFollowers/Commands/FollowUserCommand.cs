using MediatR;

namespace VisualVibes.App.UserFollowers.Commands
{
    public record FollowUserCommand(Guid FollowerId, Guid FollowingId) : IRequest<Unit>;
}
