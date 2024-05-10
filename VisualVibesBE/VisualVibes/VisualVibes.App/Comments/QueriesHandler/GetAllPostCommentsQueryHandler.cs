using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Comments.CommandsHandler;
using VisualVibes.App.Comments.Queries;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;

namespace VisualVibes.App.Comments.QueriesHandler
{
    public class GetAllPostCommentsQueryHandler : IRequestHandler<GetAllPostCommentsQuery, ICollection<ResponseCommentDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCommentCommandHandler> _logger;
        private readonly IMapper _mapper;

        public GetAllPostCommentsQueryHandler(IUnitOfWork unitOfWork, ILogger<CreateCommentCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ICollection<ResponseCommentDto>> Handle(GetAllPostCommentsQuery request, CancellationToken cancellationToken)
        {
            var comments = await _unitOfWork.CommentRepository.GetAllAsync(request.PostId);

            if (comments.Count == 0)
            {
                throw new CommentsNotFoundException($"Could not get the comments from PostId {request.PostId}, because it doesn't have any yet!");
            }

            var commentDtos = _mapper.Map<ICollection<ResponseCommentDto>>(comments);

            _logger.LogInformation("All post comments successfully retrieved!");

            return commentDtos;
        }
    }
}
