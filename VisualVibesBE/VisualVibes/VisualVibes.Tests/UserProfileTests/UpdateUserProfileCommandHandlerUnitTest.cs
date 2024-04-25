using Moq;
using VisualVibes.App.DTOs;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserProfiles.Commands;
using VisualVibes.App.UserProfiles.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.UserProfileTests
{
    public class UpdateUserProfileCommandHandlerUnitTest
    {
        private UpdateUserProfileCommandHandler _updateUserProfileCommandHandler;
        private readonly Mock<IUserProfileRepository> _userProfileRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        public UpdateUserProfileCommandHandlerUnitTest()
        {
            _userProfileRepositoryMock = new Mock<IUserProfileRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _unitOfWorkMock.Setup(uow => uow.UserProfileRepository).Returns(_userProfileRepositoryMock.Object);
            _updateUserProfileCommandHandler = new UpdateUserProfileCommandHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async void Should_UpdateUserProfile_Correctly()
        {
            //Arrange
            var userProfileDto = new UserProfileDto
            {
                Id = Guid.NewGuid(),
                ProfilePicture = "ProfilePicture1.jpg",
                DateOfBirth = new DateTime(1998, 08, 21),
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Email = "test.test@gmail.com",
                Bio = "TestBio"
            };

            var updatedUserProfileDto = new UserProfileDto
            {
                Id = userProfileDto.Id,
                ProfilePicture = userProfileDto.ProfilePicture,
                DateOfBirth = userProfileDto.DateOfBirth,
                FirstName = userProfileDto.FirstName,
                LastName = userProfileDto.LastName,
                Email = "updatedTest@gmail.com",
                Bio = "This is an updated Test bio"
            };

            var userProfile = new UserProfile
            {
                Id = updatedUserProfileDto.Id,
                ProfilePicture = updatedUserProfileDto.ProfilePicture,
                DateOfBirth = updatedUserProfileDto.DateOfBirth,
                FirstName = updatedUserProfileDto.FirstName,
                LastName = updatedUserProfileDto.LastName,
                Email = updatedUserProfileDto.Email,
                Bio = updatedUserProfileDto.Bio
            };

            var updateUserProfileCommand = new UpdateUserProfileCommand(updatedUserProfileDto);

            _userProfileRepositoryMock
                .Setup(x => x.GetByIdAsync(userProfileDto.Id))
                .ReturnsAsync(userProfile);

            _userProfileRepositoryMock
                .Setup(x => x.UpdateAsync(It.Is<UserProfile>(y => y.Id == updatedUserProfileDto.Id &&
                y.ProfilePicture == updatedUserProfileDto.ProfilePicture && y.DateOfBirth == updatedUserProfileDto.DateOfBirth &&
                y.FirstName == updatedUserProfileDto.FirstName && y.LastName == updatedUserProfileDto.LastName &&
                y.Email == updatedUserProfileDto.Email && y.Bio == updatedUserProfileDto.Bio))).ReturnsAsync(userProfile);

            //Act
            var result = await _updateUserProfileCommandHandler.Handle(updateUserProfileCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(userProfile.ProfilePicture, result.ProfilePicture);
            Assert.Equal(userProfile.DateOfBirth, result.DateOfBirth);
            Assert.Equal(userProfile.FirstName, result.FirstName);
            Assert.Equal(userProfile.LastName, result.LastName);
            Assert.Equal(userProfile.Email, result.Email);
            Assert.Equal(userProfile.Bio, result.Bio);
            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
        }
    }
}
