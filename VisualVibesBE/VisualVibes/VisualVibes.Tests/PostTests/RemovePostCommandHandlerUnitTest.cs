using Moq;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;
using VisualVibes.App.Posts.CommandsHandler;
using VisualVibes.App.Users.Commands;
using VisualVibes.App.Users.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.PostTests
{
    public class RemovePostCommandHandlerUnitTest
    {
        private RemovePostCommandHandler _removePostCommandHandler;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public RemovePostCommandHandlerUnitTest()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(uow => uow.PostRepository).Returns(_postRepositoryMock.Object);
            _removePostCommandHandler = new RemovePostCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async void Should_RemovePost_Correctly()
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

            var removePostCommand = new RemovePostCommand(postDto.Id);

            _postRepositoryMock
                .Setup(x => x.GetByIdAsync(postDto.Id)).ReturnsAsync(post);

            //Act
            var result = await _removePostCommandHandler.Handle(removePostCommand, new CancellationToken());

            //Assert
            _postRepositoryMock.Verify(x => x.GetByIdAsync(postDto.Id), Times.Once);
            _postRepositoryMock.Verify(x => x.RemoveAsync(post), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
