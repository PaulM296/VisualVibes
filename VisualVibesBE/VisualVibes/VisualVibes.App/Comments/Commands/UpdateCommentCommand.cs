using MediatR;
using VisualVibes.App.DTOs.CommentDtos;

namespace VisualVibes.App.Comments.Commands
{
    public record UpdateCommentCommand(string userId, Guid id, UpdateCommentDto updateCommentDto) : IRequest<ResponseCommentDto>;
}
