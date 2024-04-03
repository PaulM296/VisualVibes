using MediatR;
using VisualVibes.App.Users.Responses;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.Commands
{
    public record UpdateUser(Guid Id, string Username, string Password, Feed UserFeed,
        List<User> Followers, List<User> Following) : IRequest<UserDto>;
}

