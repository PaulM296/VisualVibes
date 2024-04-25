using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Users.Queries
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<UserDto>;
}
