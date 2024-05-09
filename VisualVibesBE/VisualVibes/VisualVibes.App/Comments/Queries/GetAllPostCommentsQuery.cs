using MediatR;
using VisualVibes.App.DTOs.CommentDtos;

namespace VisualVibes.App.Comments.Queries
{
    public record GetAllPostCommentsQuery(Guid PostId) : IRequest<ICollection<ResponseCommentDto>>;
}
