using Moq;
using VisualVibes.App.DTOs;
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
        public UpdateUserCommandHandlerUnitTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _updateUserCommandHandler = new UpdateUserCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async void Should_UpdateUser_Correctly()
        {
            //Arrange
            var userDto = new UserDto
            {
                Id = Guid.NewGuid(),
                Username = "UserTest",
                Password = "password123",
            };

            var updatedUserDto = new UserDto
            {
                Id = userDto.Id,
                Username = "UpdatedUserTest",
                Password = userDto.Password
            };

            var user = new User
            {
                Id = updatedUserDto.Id,
                Username = updatedUserDto.Username,
                Password = updatedUserDto.Password,
            };

            var updateUserCommand = new UpdateUserCommand(updatedUserDto);

            _userRepositoryMock
                .Setup(x => x.UpdateAsync(It.Is<User>(y => y.Username == updatedUserDto.Username &&
                y.Password == updatedUserDto.Password))).ReturnsAsync(user);

            //Act
            var result = await _updateUserCommandHandler.Handle(updateUserCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Password, result.Password);

        }
    }
}
