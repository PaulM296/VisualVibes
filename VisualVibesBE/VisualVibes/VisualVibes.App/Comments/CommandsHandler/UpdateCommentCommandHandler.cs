﻿using MediatR;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Comments.CommandsHandler
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, CommentDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCommentCommandHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<CommentDto> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            var getComment = await _unitOfWork.CommentRepository.GetByIdAsync(request.CommentDto.Id);

            getComment.Text = request.CommentDto.Text;
            getComment.CreatedAt = request.CommentDto.CreatedAt;

            var updatedComment = await _unitOfWork.CommentRepository.UpdateAsync(getComment);
            await _unitOfWork.SaveAsync(); 

            return CommentDto.FromComment(updatedComment);
        }
    }
}