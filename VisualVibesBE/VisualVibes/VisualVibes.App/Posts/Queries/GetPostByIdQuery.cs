using MediatR;
using VisualVibes.App.DTOs;

namespace VisualVibes.App.Posts.Queries
{
    public record GetPostByIdQuery(Guid PostId) : IRequest<PostDto>;
}
