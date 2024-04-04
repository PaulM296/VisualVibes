using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Comments.Queries
{
    public record GetAllPostCommentsCommand(Guid PostId) : IRequest<ICollection<CommentDto>>;
}
