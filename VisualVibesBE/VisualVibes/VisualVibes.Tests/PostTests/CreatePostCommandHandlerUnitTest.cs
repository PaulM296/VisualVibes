using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.DTOs.PostDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;
using VisualVibes.App.Posts.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.PostTests
{
    public class CreatePostCommandHandlerUnitTest
    {
        private CreatePostCommandHandler _createPostCommandHandler;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<CreatePostCommandHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFeedPostRepository> _feedPostRepositoryMock;

        public CreatePostCommandHandlerUnitTest()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CreatePostCommandHandler>>();
            _mapperMock = new Mock<IMapper>();
            _feedPostRepositoryMock = new Mock<IFeedPostRepository>();
            
            _unitOfWorkMock.Setup(uow => uow.FeedPostRepository).Returns(_feedPostRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.PostRepository).Returns(_postRepositoryMock.Object);
            
            _createPostCommandHandler = new CreatePostCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_CreatePost_Correctly()
        {
            //Arrange
            var postDto = new CreatePostDto
            {
                UserId = Guid.NewGuid(),
                Pictures = "Picture1.png",
                Caption = "This is a test caption"
            };


            var post = new Post
            {
                UserId = postDto.UserId,
                Pictures = postDto.Pictures,
                Caption = postDto.Caption,
                CreatedAt = DateTime.UtcNow
            };

            var responsePostDto = new ResponsePostDto
            {
                UserId = post.UserId,
                Pictures = post.Pictures,
                Caption = post.Caption,
                CreatedAt = post.CreatedAt
            };

            var createPostCommand = new CreatePostCommand(postDto);

            _mapperMock
                .Setup(m => m.Map<ResponsePostDto>(It.IsAny<Post>()))
                .Returns(responsePostDto);

            _postRepositoryMock
                .Setup(x => x.AddAsync(It.Is<Post>(y => y.UserId == postDto.UserId
                && y.Pictures == postDto.Pictures && y.Caption == postDto.Caption))).ReturnsAsync(post);

            _feedPostRepositoryMock.Setup(repo => repo.AddPostToFeedAsync(It.IsAny<Guid>()))
                       .Returns(Task.CompletedTask);

            //Act
            var result = await _createPostCommandHandler.Handle(createPostCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(post.UserId, result.UserId);
            Assert.Equal(post.Pictures, result.Pictures);
            Assert.Equal(post.Caption, result.Caption);
            _postRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Post>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<ResponsePostDto>(It.IsAny<Post>()), Times.Once);
        }
    }
}
