using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.Commands
{
    public record CreateUserCommand(UserDto UserDto) : IRequest<UserDto>;
}
