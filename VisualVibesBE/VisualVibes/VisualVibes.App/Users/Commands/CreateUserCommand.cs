using MediatR;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.Commands
{
    public record CreateUserCommand(RequestUserDto UserDto) : IRequest<ResponseUserDto>;
}
