using MediatR;

namespace VisualVibes.App.Comments.Queries
{
    public record GetTotalCommentNumberForPostQuery(Guid postId) : IRequest<int>; 
}
