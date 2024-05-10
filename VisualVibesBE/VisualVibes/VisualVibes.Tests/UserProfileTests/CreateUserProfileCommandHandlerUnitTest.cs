//using Moq;
//using VisualVibes.App.DTOs;
//using VisualVibes.App.Interfaces;
//using VisualVibes.App.UserProfiles.Commands;
//using VisualVibes.App.UserProfiles.CommandsHandler;
//using VisualVibes.App.Users.Commands;
//using VisualVibes.Domain.Models.BaseEntity;

//namespace VisualVibes.Tests.UserProfileTests
//{
//    public class CreateUserProfileCommandHandlerUnitTest
//    {
//        private CreateUserProfileCommandHandler _createUserProfileCommandHandler;
//        private readonly Mock<IUserProfileRepository> _userProfileRepositoryMock;
//        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
//        public CreateUserProfileCommandHandlerUnitTest()
//        {
//            _userProfileRepositoryMock = new Mock<IUserProfileRepository>();
//            _unitOfWorkMock = new Mock<IUnitOfWork>();

//            _unitOfWorkMock.Setup(uow => uow.UserProfileRepository).Returns(_userProfileRepositoryMock.Object);
//            _createUserProfileCommandHandler = new CreateUserProfileCommandHandler(_unitOfWorkMock.Object);
//        }

//        [Fact]
//        public async void Should_CreateUserProfile_Correctly()
//        {
//            //Arrange
//            var userProfileDto = new UserProfileDto
//            {
//                Id = Guid.NewGuid(),
//                ProfilePicture = "ProfilePicture1.jpg",
//                DateOfBirth = new DateTime(1998, 08, 21),
//                FirstName = "TestFirstName",
//                LastName = "TestLastName",
//                Email = "test.test@gmail.com",
//                Bio = "TestBio"
//            };

//            var user = new User
//            {
//                Id = userProfileDto.UserId,
//                Username = "ExistingUser",
//                Password = "ExistingPassword"
//            };

//            var userProfile = new UserProfile
//            {
//                Id = userProfileDto.Id,
//                ProfilePicture = userProfileDto.ProfilePicture,
//                DateOfBirth = userProfileDto.DateOfBirth,
//                FirstName = userProfileDto.FirstName,
//                LastName = userProfileDto.LastName,
//                Email = userProfileDto.Email,
//                Bio = userProfileDto.Bio
//            };

//            var createUserProfileCommand = new CreateUserProfileCommand(userProfileDto);

//            _unitOfWorkMock.Setup(uow => uow.UserRepository.GetByIdAsync(userProfileDto.UserId))
//                .ReturnsAsync(user);

//            _userProfileRepositoryMock
//                .Setup(x => x.AddAsync(It.Is<UserProfile>( y => y.ProfilePicture == userProfileDto.ProfilePicture &&
//                y.DateOfBirth == userProfileDto.DateOfBirth && y.FirstName == userProfileDto.FirstName && 
//                y.LastName == userProfileDto.LastName && y.Email == userProfileDto.Email &&
//                y.Bio == userProfileDto.Bio))).ReturnsAsync(userProfile);

//            //Act
//            var result = await _createUserProfileCommandHandler.Handle(createUserProfileCommand, new CancellationToken());

//            //Assert
//            Assert.NotNull(result);
//            Assert.Equal(userProfile.ProfilePicture, result.ProfilePicture);
//            Assert.Equal(userProfile.DateOfBirth, result.DateOfBirth);
//            Assert.Equal(userProfile.FirstName, result.FirstName);
//            Assert.Equal(userProfile.LastName, result.LastName);
//            Assert.Equal(userProfile.Email, result.Email);
//            Assert.Equal(userProfile.Bio, result.Bio);
//            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
//        }
//    }
//}
