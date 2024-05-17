using AutoMapper;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.DTOs.UserProfileDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserProfiles.Commands;
using VisualVibes.App.UserProfiles.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.UserProfileTests
{
    public class CreateUserProfileCommandHandlerUnitTest
    {
        private readonly CreateUserProfileCommandHandler _createUserProfileCommandHandler;
        private readonly Mock<IUserProfileRepository> _userProfileRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<CreateUserProfileCommandHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public CreateUserProfileCommandHandlerUnitTest()
        {
            _userProfileRepositoryMock = new Mock<IUserProfileRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CreateUserProfileCommandHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.UserProfileRepository).Returns(_userProfileRepositoryMock.Object);

            _createUserProfileCommandHandler = new CreateUserProfileCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_CreateUserProfile_Correctly()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var userProfileDto = new CreateUserProfileDto
            {
                UserId = userId,
                ProfilePicture = "ProfilePicture1.jpg",
                DateOfBirth = new DateTime(1998, 08, 21),
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Bio = "TestBio"
            };

            var user = new User
            {
                Id = userId,
                Username = "ExistingUser",
                Password = "ExistingPassword"
            };

            var userProfile = new UserProfile
            {
                UserId = userId,
                ProfilePicture = userProfileDto.ProfilePicture,
                DateOfBirth = userProfileDto.DateOfBirth,
                FirstName = userProfileDto.FirstName,
                LastName = userProfileDto.LastName,
                Bio = userProfileDto.Bio
            };

            var createUserProfileCommand = new CreateUserProfileCommand(userProfileDto);

            _unitOfWorkMock.Setup(uow => uow.UserRepository.GetByIdAsync(userId))
                .ReturnsAsync(user);

            _userProfileRepositoryMock
                .Setup(x => x.AddAsync(It.Is<UserProfile>(up => up.UserId == userId &&
                    up.FirstName == userProfileDto.FirstName && up.LastName == userProfileDto.LastName)))
                .ReturnsAsync(userProfile);

            _mapperMock
                .Setup(m => m.Map<ResponseUserProfileDto>(It.IsAny<UserProfile>()))
                .Returns(new ResponseUserProfileDto
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    FirstName = userProfile.FirstName,
                    LastName = userProfile.LastName,
                    ProfilePicture = userProfile.ProfilePicture,
                    Bio = userProfile.Bio,
                    DateOfBirth = userProfile.DateOfBirth
                });

            //Act
            var result = await _createUserProfileCommandHandler.Handle(createUserProfileCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(userProfileDto.ProfilePicture, result.ProfilePicture);
            Assert.Equal(userProfileDto.DateOfBirth, result.DateOfBirth);
            Assert.Equal(userProfileDto.FirstName, result.FirstName);
            Assert.Equal(userProfileDto.LastName, result.LastName);
            Assert.Equal(userProfileDto.Bio, result.Bio);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
