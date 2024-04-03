using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Comments.Commands
{
    public record RemoveCommentCommand(Guid Id) : IRequest<Unit>;
}
