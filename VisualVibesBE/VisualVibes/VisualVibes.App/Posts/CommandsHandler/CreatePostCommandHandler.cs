using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Posts.CommandsHandler
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, ResponsePostDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreatePostCommandHandler> _logger;
        private readonly IMapper _mapper;
        public CreatePostCommandHandler(IUnitOfWork unitOfWork, ILogger<CreatePostCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<ResponsePostDto> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = new Post()
            {
                UserId = request.userId,
                Caption = request.createPostDto.Caption,
                Pictures = request.createPostDto.Pictures,
                CreatedAt = DateTime.UtcNow
            };

            var createdPost = await _unitOfWork.PostRepository.AddAsync(post);

            await _unitOfWork.FeedPostRepository.AddPostToFeedAsync(createdPost.Id);

            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Post successfully created!");

            return _mapper.Map<ResponsePostDto>(createdPost);
        }
    }
}
