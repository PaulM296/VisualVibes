using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.FeedDtos;
using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.App.Feeds.Queries;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Feeds.QueriesHandlers
{
    public class GetUserFeedByUserIdQueryHandler : IRequestHandler<GetUserFeedByUserIdQuery, PaginationResponseDto<ResponseFeedDto>>
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

        public async Task<PaginationResponseDto<ResponseFeedDto>> Handle(GetUserFeedByUserIdQuery request, CancellationToken cancellationToken)
        {
            var pagedFeedPosts = await _unitOfWork.FeedRepository.GetPagedFeedPostsByUserIdAsync(
                request.userId, request.paginationRequestDto.PageIndex, request.paginationRequestDto.PageSize);

            if (pagedFeedPosts.Items == null || pagedFeedPosts.Items.Count == 0)
            {
                return new PaginationResponseDto<ResponseFeedDto>(new List<ResponseFeedDto>(), request.paginationRequestDto.PageIndex, pagedFeedPosts.TotalPages);
            }

            var responseFeedDto = new ResponseFeedDto
            {
                UserID = request.userId,
                Posts = pagedFeedPosts.Items.Select(fp => _mapper.Map<FeedPostDto>(fp)).ToList()
            };

            var responsePaginationDto = new PaginationResponseDto<ResponseFeedDto>(
                new List<ResponseFeedDto> { responseFeedDto },
                pagedFeedPosts.PageIndex,
                pagedFeedPosts.TotalPages
            );

            _logger.LogInformation($"Feed for user {request.userId} retrieved successfully.");

            return responsePaginationDto;
        }
    }
}
