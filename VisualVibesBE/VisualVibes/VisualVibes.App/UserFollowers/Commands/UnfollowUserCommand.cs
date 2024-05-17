using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.UserFollowers.Commands
{
    public record UnfollowUserCommand(string FollowerId, string FollowingId) : IRequest<Unit>;
}
