using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Posts.Commands
{
    public record UpdatePostCommand(PostDto PostDto) : IRequest<PostDto>;
}
