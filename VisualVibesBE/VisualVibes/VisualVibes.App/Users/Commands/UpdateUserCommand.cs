using MediatR;
using VisualVibes.App.DTOs.UserDtos;

namespace VisualVibes.App.Users.Commands
{
    public record UpdateUserCommand(Guid userId, UpdateUserDto updateUserDto) : IRequest<ResponseUserDto>;
}

