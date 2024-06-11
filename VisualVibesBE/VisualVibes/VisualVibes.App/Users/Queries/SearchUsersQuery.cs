using MediatR;
using VisualVibes.App.DTOs.UserDtos;

namespace VisualVibes.App.Users.Queries
{
    public record SearchUsersQuery(string query) : IRequest<IEnumerable<ResponseUserDto>>;
}
