using MediatR;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;
using VisualVibes.App.Users.Queries;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Posts.CommandsHandler
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, ResponsePostDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdatePostCommandHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponsePostDto> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var getPost = await _unitOfWork.PostRepository.GetByIdAsync(request.postId);

            if(getPost ==  null)
            {
                throw new PostNotFoundException($"The post with ID {request.postId} doesn't exist and it could not be updated!");
            }

            getPost.Caption = request.requestPostDto.Caption;
            getPost.Pictures = request.requestPostDto.Pictures;
            

            var updatedPost = await _unitOfWork.PostRepository.UpdateAsync(getPost);
            await _unitOfWork.SaveAsync();

            return ResponsePostDto.FromPost(updatedPost);
        }
    }
}
