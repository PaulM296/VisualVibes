using MediatR;
using VisualVibes.App.DTOs.UserDtos;

namespace VisualVibes.App.Users.Queries
{
    public record GetUserByIdQuery(string UserId) : IRequest<ResponseUserDto>;
}
