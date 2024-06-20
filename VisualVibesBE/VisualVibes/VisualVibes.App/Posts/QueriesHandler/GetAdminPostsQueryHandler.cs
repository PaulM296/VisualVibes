using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Queries;

namespace VisualVibes.App.Posts.QueriesHandler
{
    public class GetAdminPostsQueryHandler : IRequestHandler<GetAdminPostsQuery, PaginationResponseDto<JoinedResponsePostDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetAdminPostsQueryHandler> _logger;

        public GetAdminPostsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetAdminPostsQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PaginationResponseDto<JoinedResponsePostDto>> Handle(GetAdminPostsQuery request, CancellationToken cancellationToken)
        {
            var paginatedPosts = await _unitOfWork.PostRepository.GetPaginatedAdminPostsAsync(request.paginationRequest.PageIndex, request.paginationRequest.PageSize);

            if (paginatedPosts.Items.Count == 0)
            {
                throw new PostNotFoundException("No posts found for admins.");
            }

            var mappedPosts = _mapper.Map<List<JoinedResponsePostDto>>(paginatedPosts.Items);

            var response = new PaginationResponseDto<JoinedResponsePostDto>(
                items: mappedPosts,
                pageIndex: paginatedPosts.PageIndex,
                totalPages: paginatedPosts.TotalPages
            );

            _logger.LogInformation("Admin posts successfully retrieved.");

            return response;
        }
    }
}
