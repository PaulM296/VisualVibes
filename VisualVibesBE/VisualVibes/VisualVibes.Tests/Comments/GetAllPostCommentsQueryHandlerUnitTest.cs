using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.Comments.Queries;
using VisualVibes.App.Comments.QueriesHandler;
using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Messages.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.Comments
{
    public class GetAllPostCommentsQueryHandlerUnitTest
    {
        private readonly Mock<ICommentRepository> _commentRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetAllPostCommentsQueryHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllPostCommentsQueryHandler _getAllPostCommentsQueryHandler;

        public GetAllPostCommentsQueryHandlerUnitTest()
        {
            _commentRepositoryMock = new Mock<ICommentRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<GetAllPostCommentsQueryHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.CommentRepository).Returns(_commentRepositoryMock.Object);
            _getAllPostCommentsQueryHandler = new GetAllPostCommentsQueryHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_GetAllPostComments_Correctly()
        {
            //Arrange
            var postId = Guid.NewGuid();

            var comments = new List<Comment>
            {
                new Comment 
                { 
                    Id = Guid.NewGuid(), 
                    UserId = Guid.NewGuid(),
                    PostId = postId, 
                    Text = "This is a test comment", 
                    CreatedAt = DateTime.UtcNow 
                },

                new Comment 
                { 
                    Id = Guid.NewGuid(), 
                    UserId = Guid.NewGuid(), 
                    PostId = postId, 
                    Text = "Another test comment", 
                    CreatedAt = DateTime.UtcNow 
                }
            };

            var commentsDtos = new List<ResponseCommentDto>
            {
                new ResponseCommentDto 
                { 
                    Id = comments[0].Id, 
                    UserId = comments[0].UserId, 
                    PostId = postId, Text = comments[0].Text, 
                    CreatedAt = comments[0].CreatedAt 
                },

                new ResponseCommentDto 
                { 
                    Id = comments[1].Id, 
                    UserId = comments[1].UserId, 
                    PostId = postId, Text = comments[1].Text, 
                    CreatedAt = comments[1].CreatedAt 
                }
            };

            var getAllPostCommentsQuery = new GetAllPostCommentsQuery(postId);

            _commentRepositoryMock
                .Setup(x => x.GetAllAsync(postId))
                .ReturnsAsync(comments);

            _mapperMock
                .Setup(m => m.Map<ICollection<ResponseCommentDto>>(comments))
                .Returns(commentsDtos);

            //Act
            var result = await _getAllPostCommentsQueryHandler.Handle(getAllPostCommentsQuery, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(commentsDtos.Count, result.Count);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Never);
            _mapperMock.Verify(m => m.Map<ICollection<ResponseCommentDto>>(comments), Times.Once);
        }
    }
}
