using MediatR;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.App.Exceptions;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Queries;

namespace VisualVibes.App.Posts.QueriesHandler
{
    public class GetPostByIdQueryHandler : IRequestHandler<GetPostByIdQuery, ResponsePostDto>
    {
        public readonly IUnitOfWork _unitOfWork;

        public GetPostByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponsePostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(request.PostId);

            if (post == null)
            {
                throw new PostNotFoundException($"Could not get the post with Id {request.PostId}, because it doesn't exist!");
            }

            return ResponsePostDto.FromPost(post);
        }
    }
}
