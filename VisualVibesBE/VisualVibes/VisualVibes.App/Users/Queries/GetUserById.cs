using MediatR;
using VisualVibes.App.Users.Responses;

namespace VisualVibes.App.Users.Queries
{
    public record GetUserById(Guid userId) : IRequest<UserDto>;
}
