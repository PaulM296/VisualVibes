using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Comments.Queries
{
    public record GetAllPostCommentsQuery(Guid PostId) : IRequest<ICollection<CommentDto>>;
}
