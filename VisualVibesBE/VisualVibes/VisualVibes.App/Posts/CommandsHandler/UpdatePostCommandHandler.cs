using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;

namespace VisualVibes.App.Posts.CommandsHandler
{
    public class UpdatePostCommandHandler : IRequestHandler<UpdatePostCommand, ResponsePostDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<UpdatePostCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdatePostCommandHandler(IUnitOfWork unitOfWork, ILogger<UpdatePostCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponsePostDto> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
        {
            var getPost = await _unitOfWork.PostRepository.GetByIdAsync(request.postId);

            if(getPost ==  null)
            {
                throw new PostNotFoundException($"The post with ID {request.postId} doesn't exist and it could not be updated!");
            }

            getPost.Caption = request.updatePostDto.Caption;
            getPost.Pictures = request.updatePostDto.Pictures;
            

            var updatedPost = await _unitOfWork.PostRepository.UpdateAsync(getPost);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Post successfully updated!");

            return _mapper.Map<ResponsePostDto>(updatedPost);
        }
    }
}
