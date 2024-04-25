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
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public RemoveUserCommandHandlerUnitTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(_userRepositoryMock.Object);
            _removeUserCommandHandler = new RemoveUserCommandHandler(_unitOfWorkMock.Object);
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
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
