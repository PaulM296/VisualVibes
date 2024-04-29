using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.UserFollowers.Queries
{
    public record GetUserFollowersByIdQuery(Guid UserId) : IRequest<IEnumerable<UserFollowerDto>>;
}
