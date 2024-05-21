using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Comments.Commands
{
    public record RemoveCommentCommand(string userId, Guid Id) : IRequest<Unit>;
}
