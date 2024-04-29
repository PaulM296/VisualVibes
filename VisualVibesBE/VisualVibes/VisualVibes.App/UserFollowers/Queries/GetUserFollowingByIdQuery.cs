using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.UserFollowers.Queries
{
    public record GetUserFollowingByIdQuery(Guid UserId) : IRequest<IEnumerable<UserFollowerDto>>;
}
