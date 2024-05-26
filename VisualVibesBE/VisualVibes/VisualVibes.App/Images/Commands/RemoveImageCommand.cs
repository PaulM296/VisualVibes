using MediatR;

namespace VisualVibes.App.Images.Commands
{
    public record RemoveImageCommand(Guid id) : IRequest<Unit>;
}
