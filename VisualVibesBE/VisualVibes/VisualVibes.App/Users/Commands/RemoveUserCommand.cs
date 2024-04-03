using MediatR;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.Commands
{
    public record RemoveUserCommand(Guid Id) : IRequest<Unit>;
}
