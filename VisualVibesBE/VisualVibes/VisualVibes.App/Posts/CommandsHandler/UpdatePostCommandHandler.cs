using MediatR;
using VisualVibes.App.DTOs;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;
using VisualVibes.App.Users.Queries;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Posts.CommandsHandler
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, PostDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdatePostCommandHandler(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PostDto> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var getPost = await _unitOfWork.PostRepository.GetByIdAsync(request.PostDto.Id);

            if(getPost ==  null)
            {
                throw new PostNotFoundException($"The post with ID {request.PostDto.Id} doesn't exist and it could not be updated!");
            }

            getPost.Caption = request.PostDto.Caption;
            getPost.Pictures = request.PostDto.Pictures;

            var updatedPost = await _unitOfWork.PostRepository.UpdateAsync(getPost);
            await _unitOfWork.SaveAsync();

            return PostDto.FromPost(updatedPost);
        }
    }
}
