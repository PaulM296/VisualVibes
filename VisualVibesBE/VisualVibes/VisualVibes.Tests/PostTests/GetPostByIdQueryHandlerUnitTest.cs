using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Queries;
using VisualVibes.App.Posts.QueriesHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.PostTests
{
    public class GetPostByIdQueryHandlerUnitTest
    {
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetPostByIdQueryHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetPostByIdQueryHandler _getPostByIdQueryHandler;

        public GetPostByIdQueryHandlerUnitTest()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<GetPostByIdQueryHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.PostRepository).Returns(_postRepositoryMock.Object);
            
            _getPostByIdQueryHandler = new GetPostByIdQueryHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_GetPost_Correctly()
        {
            //Arrange
            var postDto = new ResponsePostDto
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Pictures = "Picture1.png",
                Caption = "This is a test caption",
                CreatedAt = DateTime.UtcNow
            };

            var post = new Post
            {
                Id = postDto.Id,
                UserId = postDto.UserId,
                Pictures = postDto.Pictures,
                Caption = postDto.Caption,
                CreatedAt = postDto.CreatedAt
            };


            var getPostByIdQuery = new GetPostByIdQuery(postDto.Id);

            _postRepositoryMock
                .Setup(x => x.GetByIdAsync(postDto.Id))
                .ReturnsAsync(post);

            _mapperMock
                .Setup(m => m.Map<ResponsePostDto>(It.IsAny<Post>()))
                .Returns(postDto);

            //Act
            var result = await _getPostByIdQueryHandler.Handle(getPostByIdQuery, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(post.Id, result.Id);
            Assert.Equal(post.UserId, result.UserId);
            Assert.Equal(post.Pictures, result.Pictures);
            Assert.Equal(post.Caption, result.Caption);
            Assert.Equal(post.CreatedAt, result.CreatedAt);
            _mapperMock.Verify(m => m.Map<ResponsePostDto>(post), Times.Once);
        }
    }
}
