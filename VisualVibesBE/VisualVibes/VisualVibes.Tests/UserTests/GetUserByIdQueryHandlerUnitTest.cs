using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.Users.Queries;
using VisualVibes.App.Users.QueriesHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.UserTests
{
    public class GetUserByIdQueryHandlerUnitTest
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetUserByIdQueryHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetUserByIdQueryHandler _getUserByIdQueryHandler;

        public GetUserByIdQueryHandlerUnitTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<GetUserByIdQueryHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(_userRepositoryMock.Object);

            _getUserByIdQueryHandler = new GetUserByIdQueryHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_GetUserById_Correctly()
        {
            //Arrange
            var userDto = new ResponseUserDto
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

            _mapperMock
                .Setup(m => m.Map<ResponseUserDto>(It.IsAny<User>())).Returns(userDto);

            //Act
            var result = await _getUserByIdQueryHandler.Handle(getUserByIdQuery, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(userDto.Username, result.Username);
            Assert.Equal(userDto.Password, result.Password);
            Assert.Equal(userDto.Id, result.Id);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Never);
            _userRepositoryMock.Verify(x => x.GetByIdAsync(userDto.Id), Times.Once);
            _mapperMock.Verify(m => m.Map<ResponseUserDto>(It.IsAny<User>()), Times.Once);
        }
    }
}
