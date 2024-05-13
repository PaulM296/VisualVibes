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
    public class UpdateUserCommandHandlerUnitTest
    {
        private readonly UpdateUserCommandHandler _updateUserCommandHandler;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<UpdateUserCommandHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        public UpdateUserCommandHandlerUnitTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<UpdateUserCommandHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(_userRepositoryMock.Object);

            _updateUserCommandHandler = new UpdateUserCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_UpdateUser_Correctly()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var updatedUserDto = new UpdateUserDto
            {
                Username = "UpdatedUserTest",
            };

            var user = new User
            {
                Id = userId,
                Username = "UserTest",
                Password = "password123",
            };

            var updateUserCommand = new UpdateUserCommand(userId, updatedUserDto);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

            _userRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync((User u) => u);

            _mapperMock
                .Setup(m => m.Map<ResponseUserDto>(It.IsAny<User>())).Returns((User u) => new ResponseUserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Password = u.Password
                });

            //Act
            var result = await _updateUserCommandHandler.Handle(updateUserCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(updatedUserDto.Username, result.Username);
            _userRepositoryMock.Verify(x => x.GetByIdAsync(userId), Times.Once);
            _userRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
