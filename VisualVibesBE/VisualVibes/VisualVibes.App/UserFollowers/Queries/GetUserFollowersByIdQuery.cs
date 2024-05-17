using MediatR;
using VisualVibes.App.DTOs.UserFollowerDtos;

namespace VisualVibes.App.UserFollowers.Queries
{
    public record GetUserFollowersByIdQuery(string UserId) : IRequest<IEnumerable<UserFollowerDto>>;
}
