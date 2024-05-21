using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using VisualVibes.App.Comments.Commands;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Comments.CommandsHandler
{
    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, ResponseCommentDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateCommentCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateCommentCommandHandler(IUnitOfWork unitOfWork, ILogger<CreateCommentCommandHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<ResponseCommentDto> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = new Comment()
            {
                UserId = request.userId,
                PostId = request.createCommentDto.PostId,
                Text = request.createCommentDto.Text,
                CreatedAt = DateTime.UtcNow
            };

            var createdComment = await _unitOfWork.CommentRepository.AddAsync(comment);
            await _unitOfWork.SaveAsync();

            _logger.LogInformation("Comment successfully created!");

            return _mapper.Map<ResponseCommentDto>(createdComment);
        }
    }
}
