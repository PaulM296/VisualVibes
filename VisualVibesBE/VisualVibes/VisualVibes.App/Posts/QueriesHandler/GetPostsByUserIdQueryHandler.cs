using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Queries;

namespace VisualVibes.App.Posts.QueriesHandler
{
    public class GetPostsByUserIdQueryHandler : IRequestHandler<GetPostsByUserIdQuery, IEnumerable<ResponsePostDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPostsByUserIdQueryHandler> _logger;

        public GetPostsByUserIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetPostsByUserIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<ResponsePostDto>> Handle(GetPostsByUserIdQuery request, CancellationToken cancellationToken)
        {
            var posts = await _unitOfWork.PostRepository.GetPostsByUserIdAsync(request.userId);

            return _mapper.Map<IEnumerable<ResponsePostDto>>(posts);
        }
    }
}
