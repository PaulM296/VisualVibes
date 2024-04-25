using Moq;
using VisualVibes.App.DTOs;
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
        public CreatePostCommandHandlerUnitTest()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(uow => uow.PostRepository).Returns(_postRepositoryMock.Object);
            _createPostCommandHandler = new CreatePostCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async void Should_CreatePost_Correctly()
        {
            //Arrange
            var postDto = new PostDto
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

            var createPostCommand = new CreatePostCommand(postDto);

            _postRepositoryMock
                .Setup(x => x.AddAsync(It.Is<Post>(y => y.UserId == postDto.UserId 
                && y.Pictures == postDto.Pictures && y.Caption == postDto.Caption 
                && y.CreatedAt == postDto.CreatedAt))).ReturnsAsync(post);

            //Act
            var result = await _createPostCommandHandler.Handle(createPostCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(post.UserId, result.UserId);
            Assert.Equal(post.Pictures, result.Pictures);
            Assert.Equal(post.Caption, result.Caption);
            Assert.Equal(post.CreatedAt, result.CreatedAt);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
