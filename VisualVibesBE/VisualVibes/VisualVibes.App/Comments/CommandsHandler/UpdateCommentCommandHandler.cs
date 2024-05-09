using MediatR;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Comments.CommandsHandler
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, ResponseCommentDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCommentCommandHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseCommentDto> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var getComment = await _unitOfWork.CommentRepository.GetByIdAsync(request.id);

            if (getComment == null)
            {
                throw new CommentsNotFoundException($"The comment with ID {request.id} doesn't exist and it could not be updated!");
            }

            getComment.Text = request.updateCommentDto.Text;

            var updatedComment = await _unitOfWork.CommentRepository.UpdateAsync(getComment);
            await _unitOfWork.SaveAsync(); 

            return ResponseCommentDto.FromComment(updatedComment);
        }
    }
}