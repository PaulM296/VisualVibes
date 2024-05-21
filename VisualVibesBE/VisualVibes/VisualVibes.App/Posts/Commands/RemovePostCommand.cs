using MediatR;

namespace VisualVibes.App.Posts.Commands
{
    public record RemovePostCommand(string userId, Guid Id) : IRequest<Unit>;
}
