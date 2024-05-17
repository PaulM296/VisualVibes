using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.App.Users.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.UserTests
{
    public class CreateUserCommandHandlerUnitTest
    {

        private readonly RegisterUserCommandHandler _createUserCommandHandler;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<RegisterUserCommandHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IFeedRepository> _feedRepositoryMock;
        private readonly Mock<IFeedPostRepository> _feedPostRepositoryMock;

        public CreateUserCommandHandlerUnitTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<RegisterUserCommandHandler>>();
            _mapperMock = new Mock<IMapper>();
            _feedRepositoryMock = new Mock<IFeedRepository>();
            _feedPostRepositoryMock = new Mock<IFeedPostRepository>();

            _unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(_userRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.FeedRepository).Returns(_feedRepositoryMock.Object);
            _unitOfWorkMock.Setup(uow => uow.FeedPostRepository).Returns(_feedPostRepositoryMock.Object);

            _createUserCommandHandler = new CreateUserCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_CreateUser_Correctly()
        {
            //Arrange
            var userDto = new RegisterUser
            {
                Username = "UserTest",
                Password = "password123"
            };

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = userDto.Username,
                Password = userDto.Password
            };

            var createUserCommand = new RegisterUserCommand(userDto);

            _userRepositoryMock
                .Setup(x => x.AddAsync(It.Is<User>(y => y.Username == userDto.Username &&
                y.Password == userDto.Password))).ReturnsAsync(user);

            _feedRepositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Feed>())).ReturnsAsync(new Feed 
            { 
                UserID = user.Id 
            });

            _feedPostRepositoryMock
                .Setup(repo => repo.EnsureFeedForUserAsync(It.IsAny<Guid>())).Returns(Task.CompletedTask);

            _mapperMock
                .Setup(m => m.Map<ResponseUserDto>(It.IsAny<User>())).Returns(new ResponseUserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Password = user.Password
                });

            //Act
            var result = await _createUserCommandHandler.Handle(createUserCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Password, result.Password);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<ResponseUserDto>(It.IsAny<User>()), Times.Once);
            _feedRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Feed>()), Times.Once);
            _feedPostRepositoryMock.Verify(repo => repo.EnsureFeedForUserAsync(It.IsAny<Guid>()), Times.Once);
        }

    }
}
