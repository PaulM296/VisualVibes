using MediatR;
using VisualVibes.App.DTOs.UserDtos;

namespace VisualVibes.App.Users.Commands
{
    public record LoginUserCommand(LoginDto loginUser) : IRequest<string>;
}
