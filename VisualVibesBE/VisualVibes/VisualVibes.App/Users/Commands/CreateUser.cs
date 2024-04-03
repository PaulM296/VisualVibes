using MediatR;
using VisualVibes.App.Users.Responses;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.Commands
{
    public record CreateUser(string Username, string Password,UserProfile UserProfile,
        Feed UserFeed, List<User> Followers, List<User> Following) : IRequest<UserDto>;
}
