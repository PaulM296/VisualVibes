using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Comments.CommandsHandler
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, ResponseCommentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCommentCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateCommentCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateCommentCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
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

            _logger.LogInformation("Comment successfully updated!");

            return _mapper.Map<ResponseCommentDto>(updatedComment);
        }
    }
}