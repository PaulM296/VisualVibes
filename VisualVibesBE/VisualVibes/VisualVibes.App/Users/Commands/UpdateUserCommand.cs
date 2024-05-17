using MediatR;
using VisualVibes.App.DTOs.UserDtos;

namespace VisualVibes.App.Users.Commands
{
    public record UpdateUserCommand(string userId, UpdateUserDto updateUserDto) : IRequest<ResponseUserDto>;
}

