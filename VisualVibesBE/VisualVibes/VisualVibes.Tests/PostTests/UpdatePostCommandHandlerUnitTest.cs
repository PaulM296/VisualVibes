using Moq;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;
using VisualVibes.App.Posts.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.PostTests
{
    public class UpdatePostCommandHandlerUnitTest
    {
        private UpdatePostCommandHandler _updatePostCommandHandler;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        public UpdatePostCommandHandlerUnitTest()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(uow => uow.PostRepository).Returns(_postRepositoryMock.Object);
            _updatePostCommandHandler = new UpdatePostCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async void Should_UpdatePost_Correctly()
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

            var updatedPostDto = new PostDto
            {
                Id = postDto.Id,
                UserId = postDto.UserId,
                Pictures = "Picture2.png",
                Caption = "Updated test caption",
                CreatedAt = postDto.CreatedAt,
            };

            var post = new Post
            {
                Id = updatedPostDto.Id,
                UserId = updatedPostDto.UserId,
                Pictures = updatedPostDto.Pictures,
                Caption = updatedPostDto.Caption,
                CreatedAt = updatedPostDto.CreatedAt
            };

            var updatePostCommand = new UpdatePostCommand(updatedPostDto);

            _postRepositoryMock
            .Setup(x => x.GetByIdAsync(postDto.Id)).ReturnsAsync(post);
            _postRepositoryMock
                .Setup(x => x.UpdateAsync(It.Is<Post>(y => y.UserId == updatedPostDto.UserId
                && y.Pictures == updatedPostDto.Pictures && y.Caption == updatedPostDto.Caption
                && y.CreatedAt == updatedPostDto.CreatedAt))).ReturnsAsync(post);

            //Act
            var result = await _updatePostCommandHandler.Handle(updatePostCommand, new CancellationToken());

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
