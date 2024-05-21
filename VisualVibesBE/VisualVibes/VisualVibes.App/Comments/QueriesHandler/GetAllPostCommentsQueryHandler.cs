using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Comments.Queries;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.DTOs.PaginationDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Comments.QueriesHandler
{
    public class GetAllPostCommentsQueryHandler : IRequestHandler<GetAllPostCommentsQuery, PaginationResponseDto<ResponseCommentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GetAllPostCommentsQueryHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllPostCommentsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllPostCommentsQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<PaginationResponseDto<ResponseCommentDto>> Handle(GetAllPostCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await _unitOfWork.CommentRepository
                .GetAllPagedCommentsAsync(request.PostId, request.paginationRequestDto.PageIndex, request.paginationRequestDto.PageSize);

            if (comments.Items.Count == 0)
            {
                throw new CommentsNotFoundException($"Could not get the comments from PostId {request.PostId}, because it doesn't have any yet!");
            }

            var commentDtos = new PaginationResponseDto<ResponseCommentDto>(
                items: _mapper.Map<List<ResponseCommentDto>>(comments.Items),
                pageIndex: comments.PageIndex,
                totalPages: comments.TotalPages);

            _logger.LogInformation("All post comments successfully retrieved!");

            return commentDtos;
        }
    }
}
