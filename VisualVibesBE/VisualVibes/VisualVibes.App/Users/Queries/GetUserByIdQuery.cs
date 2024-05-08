using MediatR;
using VisualVibes.App.DTOs.UserDtos;

namespace VisualVibes.App.Users.Queries
{
    public record GetUserByIdQuery(Guid UserId) : IRequest<ResponseUserDto>;
}
