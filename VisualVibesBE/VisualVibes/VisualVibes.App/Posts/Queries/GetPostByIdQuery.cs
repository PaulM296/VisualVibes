using MediatR;
using VisualVibes.App.DTOs.PostDtos;

namespace VisualVibes.App.Posts.Queries
{
    public record GetPostByIdQuery(Guid PostId) : IRequest<ResponsePostDto>;
}
