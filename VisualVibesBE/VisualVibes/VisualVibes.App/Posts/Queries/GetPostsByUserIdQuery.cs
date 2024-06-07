using MediatR;
using VisualVibes.App.DTOs.PostDtos;

namespace VisualVibes.App.Posts.Queries
{
    public record GetPostsByUserIdQuery(string userId) : IRequest<IEnumerable<ResponsePostDto>>;
}
