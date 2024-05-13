using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Posts.Commands;
using VisualVibes.App.Posts.CommandsHandler;
using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.PostTests
{
    public class RemovePostCommandHandlerUnitTest
    {
        private RemovePostCommandHandler _removePostCommandHandler;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IFeedPostRepository> _feedPostRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<RemovePostCommandHandler>> _loggerMock;

        public RemovePostCommandHandlerUnitTest()
        {
            _postRepositoryMock = new Mock<IPostRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<RemovePostCommandHandler>>();
            _feedPostRepositoryMock = new Mock<IFeedPostRepository>();

            _unitOfWorkMock.Setup(uow => uow.PostRepository).Returns(_postRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.FeedPostRepository).Returns(_feedPostRepositoryMock.Object);

            _removePostCommandHandler = new RemovePostCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async void Should_RemovePost_Correctly()
        {
            //Arrange
            var postId = Guid.NewGuid();

            var post = new Post
            {
                Id = postId,
                UserId = Guid.NewGuid(),
                Pictures = "Picture1.png",
                Caption = "This is a test caption",
                CreatedAt = DateTime.UtcNow
            };


            var removePostCommand = new RemovePostCommand(postId);

            _postRepositoryMock
                .Setup(x => x.GetByIdAsync(postId)).ReturnsAsync(post);

            _feedPostRepositoryMock
                .Setup(repo => repo.GetByPostIdAsync(postId))
                .ReturnsAsync(new List<FeedPost>());

            //Act
            var result = await _removePostCommandHandler.Handle(removePostCommand, new CancellationToken());

            //Assert
            _postRepositoryMock.Verify(x => x.GetByIdAsync(postId), Times.Once);
            _postRepositoryMock.Verify(x => x.RemoveAsync(post), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
