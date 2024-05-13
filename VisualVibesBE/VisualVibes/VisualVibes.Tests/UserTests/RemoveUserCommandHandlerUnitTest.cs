using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.App.Users.CommandsHandler;
using VisualVibes.App.Users.QueriesHandler;
using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.UserTests
{
    public class RemoveUserCommandHandlerUnitTest
    {
        private readonly RemoveUserCommandHandler _removeUserCommandHandler;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IFeedRepository> _feedRepositoryMock;
        private readonly Mock<IFeedPostRepository> _feedPostRepositoryMock;
        private readonly Mock<IPostRepository> _postRepositoryMock;
        private readonly Mock<IUserFollowerRepository> _userFollowerRepositoryMock;
        private readonly Mock<IConversationRepository> _conversationRepositoryMock;
        private readonly Mock<ILogger<RemoveUserCommandHandler>> _loggerMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public RemoveUserCommandHandlerUnitTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _postRepositoryMock = new Mock<IPostRepository>();
            _userFollowerRepositoryMock = new Mock<IUserFollowerRepository>();
            _conversationRepositoryMock = new Mock<IConversationRepository>();
            _feedRepositoryMock = new Mock<IFeedRepository>();
            _feedPostRepositoryMock = new Mock<IFeedPostRepository>();
            _loggerMock = new Mock<ILogger<RemoveUserCommandHandler>>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.PostRepository).Returns(_postRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.UserFollowerRepository).Returns(_userFollowerRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.ConversationRepository).Returns(_conversationRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.FeedRepository).Returns(_feedRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.FeedPostRepository).Returns(_feedPostRepositoryMock.Object);

            _removeUserCommandHandler = new RemoveUserCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async void Should_RemoveUser_Correctly()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var user = new User
            {
                Id = userId,
                Username = "UserTest",
                Password = "password123"
            };

            var removeUserCommand = new RemoveUserCommand(userId);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

            _postRepositoryMock
                .Setup(x => x.GetPostsByUserIdAsync(userId)).ReturnsAsync(new List<Post>());

            _userFollowerRepositoryMock
                .Setup(x => x.GetFollowersByUserIdAsync(userId)).ReturnsAsync(new List<UserFollower>());

            _userFollowerRepositoryMock
                .Setup(x => x.GetFollowingByUserIdAsync(userId)).ReturnsAsync(new List<UserFollower>());

            _conversationRepositoryMock
                .Setup(x => x.GetAllByUserIdAsync(userId)).ReturnsAsync(new List<Conversation>());

            _feedRepositoryMock
                .Setup(x => x.GetByUserIdAsync(userId)).ReturnsAsync((Feed)null);

            _feedPostRepositoryMock
                .Setup(x => x.GetByFeedIdAsync(It.IsAny<Guid>())).ReturnsAsync(new List<FeedPost>());


            //Act
            var result = await _removeUserCommandHandler.Handle(removeUserCommand, new CancellationToken());

            //Assert
            _userRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
            _postRepositoryMock.Verify(x => x.GetPostsByUserIdAsync(userId), Times.Once);
            _userRepositoryMock.Verify(x => x.RemoveAsync(user), Times.Once);
            _userFollowerRepositoryMock.Verify(x => x.GetFollowersByUserIdAsync(userId), Times.Once);
            _userFollowerRepositoryMock.Verify(x => x.GetFollowingByUserIdAsync(userId), Times.Once);
            _conversationRepositoryMock.Verify(x => x.GetAllByUserIdAsync(userId), Times.Once);
            _feedRepositoryMock.Verify(x => x.GetByUserIdAsync(userId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);

        }
    }
}
