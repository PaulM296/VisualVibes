using Moq;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.App.Users.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.UserTests
{
    public class RemoveUserCommandHandlerUnitTest
    {
        private RemoveUserCommandHandler _removeUserCommandHandler;
        private Mock<IUserRepository> _userRepositoryMock;

        public RemoveUserCommandHandlerUnitTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _removeUserCommandHandler = new RemoveUserCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async void Should_RemoveUser_Correctly()
        {
            //Arrange
            var userDto = new UserDto
            {
                Id = Guid.NewGuid(),
                Username = "UserTest",
                Password = "password123",
            };

            var user = new User
            {
                Id = userDto.Id,
                Username = userDto.Username,
                Password = userDto.Password,
            };

            var removeUserCommand = new RemoveUserCommand(userDto.Id);

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userDto.Id)).ReturnsAsync(user);

            //Act
            var result = await _removeUserCommandHandler.Handle(removeUserCommand, new CancellationToken());

            //Assert
            _userRepositoryMock.Verify(x => x.GetByIdAsync(userDto.Id), Times.Once);
            _userRepositoryMock.Verify(x => x.RemoveAsync(user), Times.Once);
        }
    }
}
