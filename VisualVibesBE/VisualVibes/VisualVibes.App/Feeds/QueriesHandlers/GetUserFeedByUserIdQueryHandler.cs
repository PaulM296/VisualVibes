using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.FeedDtos;
using VisualVibes.App.Feeds.Queries;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Feeds.QueriesHandlers
{
    public class GetUserFeedByUserIdQueryHandler : IRequestHandler<GetUserFeedByUserIdQuery, ResponseFeedDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetUserFeedByUserIdQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetUserFeedByUserIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetUserFeedByUserIdQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseFeedDto> Handle(GetUserFeedByUserIdQuery request, CancellationToken cancellationToken)
        {
            var feed = await _unitOfWork.FeedRepository.GetFeedPostsByUserIdAsync(request.userId);

            if(feed == null)
            {
                return null;
            }

            var responseFeedDto = new ResponseFeedDto
            {
                UserID = request.userId,
                Posts = feed.Select(fp => _mapper.Map<FeedPostDto>(fp.Post)).ToList()
            };

            _logger.LogInformation($"Feed for user {request.userId} retrieved successfully.");
            return responseFeedDto;
        }
    }
}
