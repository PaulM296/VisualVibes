using Moq;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Commands;
using VisualVibes.App.Users.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.UserTests
{
    public class CreateUserCommandHandlerUnitTest
    {

        private CreateUserCommandHandler _createUserCommandHandler;
        private Mock<IUserRepository> _userRepositoryMock;

        public CreateUserCommandHandlerUnitTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _createUserCommandHandler = new CreateUserCommandHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async void Should_CreateUser_Correctly()
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

            var createUserCommand = new CreateUserCommand(userDto);

            _userRepositoryMock
                .Setup(x => x.AddAsync(It.Is<User>(y => y.Username == userDto.Username &&
                y.Password == userDto.Password))).ReturnsAsync(user);

            //Act
            var result = await _createUserCommandHandler.Handle(createUserCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(user.Username, result.Username);
            Assert.Equal(user.Password, result.Password);
        }
    }
}
