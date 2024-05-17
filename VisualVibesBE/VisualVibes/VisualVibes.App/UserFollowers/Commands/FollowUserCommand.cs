using MediatR;

namespace VisualVibes.App.UserFollowers.Commands
{
    public record FollowUserCommand(string FollowerId, string FollowingId) : IRequest<Unit>;
}
