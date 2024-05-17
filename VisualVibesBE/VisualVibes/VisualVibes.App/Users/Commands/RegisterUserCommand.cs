using MediatR;
using VisualVibes.App.DTOs.UserDtos;

namespace VisualVibes.App.Users.Commands
{
    public record RegisterUserCommand(RegisterUser registerUser) : IRequest<string>;
}
