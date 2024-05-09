using MediatR;
using VisualVibes.App.DTOs.UserFollowerDtos;

namespace VisualVibes.App.UserFollowers.Queries
{
    public record GetUserFollowingByIdQuery(Guid UserId) : IRequest<IEnumerable<UserFollowerDto>>;
}
