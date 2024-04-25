using Moq;
using VisualVibes.App.Comments.Queries;
using VisualVibes.App.Comments.QueriesHandler;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.Comments
{
    public class GetAllPostCommentsQueryHandlerUnitTest
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private GetAllPostCommentsQueryHandler _getAllPostCommentsQueryHandler;

        public GetAllPostCommentsQueryHandlerUnitTest()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(uow => uow.CommentRepository).Returns(_commentRepositoryMock.Object);
            _getAllPostCommentsQueryHandler = new GetAllPostCommentsQueryHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async void Should_GetAllPostComments_Correctly()
        {
            var postId = Guid.NewGuid();

            var commentsDtos = new List<CommentDto>
            {
                new CommentDto
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    PostId = postId,
                    Text = "This is a test comment",
                    CreatedAt = DateTime.Now,
                },

                new CommentDto
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(),
                    PostId = postId,
                    Text = "This is a test comment",
                    CreatedAt = DateTime.Now,
                }
            };

            var comments = new List<Comment>();
            foreach (var commentDto in commentsDtos)
            {
                comments.Add(new Comment
                {
                    Id = commentDto.Id,
                    UserId = commentDto.UserId,
                    PostId = commentDto.PostId,
                    Text = commentDto.Text,
                    CreatedAt = commentDto.CreatedAt
                });
            }

            var getAllPostCommentsQuery = new GetAllPostCommentsQuery(postId);

            _commentRepositoryMock
            .Setup(x => x.GetAllAsync(postId))
                .ReturnsAsync(comments);

            //Act
            var result = await _getAllPostCommentsQueryHandler.Handle(getAllPostCommentsQuery, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(commentsDtos.Count, result.Count);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Never);
        }
    }
}
