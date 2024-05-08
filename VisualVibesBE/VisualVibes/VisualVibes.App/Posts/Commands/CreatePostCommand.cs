using MediatR;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Posts.Commands
{
    public record CreatePostCommand(RequestPostDto requestPostDto) : IRequest<ResponsePostDto>;
}
