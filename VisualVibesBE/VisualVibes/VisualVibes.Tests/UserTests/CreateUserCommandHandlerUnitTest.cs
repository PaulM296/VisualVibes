//using Moq;
//using VisualVibes.App.DTOs;
//using VisualVibes.App.DTOs.UserDtos;
//using VisualVibes.App.Interfaces;
//using VisualVibes.App.Users.Commands;
//using VisualVibes.App.Users.CommandsHandler;
//using VisualVibes.Domain.Models.BaseEntity;

//namespace VisualVibes.Tests.UserTests
//{
//    public class CreateUserCommandHandlerUnitTest
//    {

//        private CreateUserCommandHandler _createUserCommandHandler;
//        private readonly Mock<IUserRepository> _userRepositoryMock;
//        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

//        public CreateUserCommandHandlerUnitTest()
//        {
//            _userRepositoryMock = new Mock<IUserRepository>();
//            _unitOfWorkMock = new Mock<IUnitOfWork>();

//            _unitOfWorkMock.Setup(uow => uow.UserRepository).Returns(_userRepositoryMock.Object);
//            _createUserCommandHandler = new CreateUserCommandHandler(_unitOfWorkMock.Object);
//        }

//        [Fact]
//        public async void Should_CreateUser_Correctly()
//        {
//            //Arrange
//            var userDto = new CreateUserDto
//            {
//                Username = "UserTest",
//                Password = "password123",
//            };

//            var user = new User
//            {
//                Username = userDto.Username,
//                Password = userDto.Password,
//            };

//            var createUserCommand = new CreateUserCommand(userDto);

//            _userRepositoryMock
//                .Setup(x => x.AddAsync(It.Is<User>(y => y.Username == userDto.Username &&
//                y.Password == userDto.Password))).ReturnsAsync(user);

//            //Act
//            var result = await _createUserCommandHandler.Handle(createUserCommand, new CancellationToken());

//            //Assert
//            Assert.NotNull(result);
//            Assert.Equal(user.Username, result.Username);
//            Assert.Equal(user.Password, result.Password);
//            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
//        }
//    }
//}
