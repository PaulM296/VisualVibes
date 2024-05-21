using MediatR;
using VisualVibes.App.DTOs.CommentDtos;

namespace VisualVibes.App.Comments.Commands
{
    public record CreateCommentCommand(string userId, CreateCommentDto createCommentDto) : IRequest<ResponseCommentDto>;
}
