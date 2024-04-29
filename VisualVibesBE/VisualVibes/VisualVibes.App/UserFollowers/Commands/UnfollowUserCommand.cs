using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.UserFollowers.Commands
{
    public record UnfollowUserCommand(Guid FollowerId, Guid FollowingId) : IRequest<Unit>;
}
