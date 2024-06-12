using MediatR;

namespace VisualVibes.App.UserFollowers.Queries
{
    public record IsFollowingUserQuery(string followerId, string followingId) : IRequest<bool>;
}
