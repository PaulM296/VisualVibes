using MediatR;
using VisualVibes.App.Users.Responses;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Users.Commands
{
    public record RemoveUser(Guid Id) : IRequest<Unit>;
}
