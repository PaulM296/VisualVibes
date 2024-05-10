using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Queries;

namespace VisualVibes.App.Posts.QueriesHandler
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, ResponsePostDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetPostByIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetPostByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetPostByIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponsePostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(request.PostId);

            if (post == null)
            {
                throw new PostNotFoundException($"Could not get the post with Id {request.PostId}, because it doesn't exist!");
            }

            _logger.LogInformation("Post successfully retrieved!");

            return _mapper.Map<ResponsePostDto>(post);
        }
    }
}
