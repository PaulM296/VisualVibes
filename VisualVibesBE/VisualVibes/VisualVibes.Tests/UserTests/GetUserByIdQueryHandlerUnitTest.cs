using Moq;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Queries;
using VisualVibes.App.Users.QueriesHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.UserTests
{
    public class GetUserByIdQueryHandlerUnitTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private GetUserByIdQueryHandler _getUserByIdQueryHandler;

        public GetUserByIdQueryHandlerUnitTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _getUserByIdQueryHandler = new GetUserByIdQueryHandler(_userRepositoryMock.Object);
        }

        [Fact]
        public async void Should_GetUserById_Correctly()
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
                Password = userDto.Password
            };

            var getUserByIdQuery = new GetUserByIdQuery(userDto.Id);

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(userDto.Id)).ReturnsAsync(user);

            //Act
            var result = await _getUserByIdQueryHandler.Handle(getUserByIdQuery, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(userDto.Username, result.Username);
            Assert.Equal(userDto.Password, result.Password);
            Assert.Equal(userDto.Id, result.Id);
        }
    }
}
