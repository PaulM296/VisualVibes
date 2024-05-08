using Moq;
using VisualVibes.App.DTOs;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.App.Users.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.UserTests
{
    public class UpdateUserCommandHandlerUnitTest
    {
        private UpdateUserCommandHandler _updateUserCommandHandler;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        public UpdateUserCommandHandlerUnitTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(_userRepositoryMock.Object);
            _updateUserCommandHandler = new UpdateUserCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async void Should_UpdateUser_Correctly()
        {
            //Arrange
            var userDto = new ResponseUserDto
            {
                Username = "UserTest",
                Password = "password123",
            };

            var updatedUserDto = new RequestUserDto
            {
                Username = "UpdatedUserTest",
                Password = userDto.Password
            };

            var user = new User
            {
                Username = updatedUserDto.Username,
                Password = updatedUserDto.Password,
            };

            var updateUserCommand = new UpdateUserCommand(Guid.NewGuid(), updatedUserDto);

            _userRepositoryMock
            .Setup(x => x.GetByIdAsync(userDto.Id)).ReturnsAsync(user);
            _userRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<User>()))
                .ReturnsAsync((User u) => u);

            //Act
            var result = await _updateUserCommandHandler.Handle(updateUserCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Password, result.Password);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
