using MediatR;

namespace VisualVibes.App.Posts.Commands
{
    public record RemovePostCommand(Guid Id) : IRequest<Unit>;
}
