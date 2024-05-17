using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.DTOs;
using VisualVibes.App.DTOs.UserProfileDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserProfiles.Commands;
using VisualVibes.App.UserProfiles.CommandsHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.UserProfileTests
{
    public class UpdateUserProfileCommandHandlerUnitTest
    {
        private readonly UpdateUserProfileCommandHandler _updateUserProfileCommandHandler;
        private readonly Mock<IUserProfileRepository> _userProfileRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<UpdateUserProfileCommandHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public UpdateUserProfileCommandHandlerUnitTest()
        {
            _userProfileRepositoryMock = new Mock<IUserProfileRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<UpdateUserProfileCommandHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.UserProfileRepository).Returns(_userProfileRepositoryMock.Object);

            _updateUserProfileCommandHandler = new UpdateUserProfileCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async void Should_UpdateUserProfile_Correctly()
        {
            //Arrange
            var userId = Guid.NewGuid();

            var userProfile = new UserProfile
            {
                UserId = userId,
                ProfilePicture = "ProfilePicture1.jpg",
                DateOfBirth = new DateTime(1998, 08, 21),
                FirstName = "TestFirstName",
                LastName = "TestLastName",
                Bio = "TestBio"
            };

            var updateUserProfileDto = new UpdateUserProfileDto
            {
                ProfilePicture = "UpdatedProfilePicture.jpg",
                DateOfBirth = userProfile.DateOfBirth.AddDays(1),
                FirstName = "UpdatedFirstName",
                LastName = "UpdatedLastName",
                Bio = "Updated Bio",
            };

            var responseUserProfileDto = new ResponseUserProfileDto
            {
                Id = userProfile.Id,
                UserId = userProfile.UserId,
                FirstName = updateUserProfileDto.FirstName,
                LastName = updateUserProfileDto.LastName,
                ProfilePicture = updateUserProfileDto.ProfilePicture,
                Bio = updateUserProfileDto.Bio,
                DateOfBirth = updateUserProfileDto.DateOfBirth
            };

            var updateUserProfileCommand = new UpdateUserProfileCommand(userId, updateUserProfileDto);

            _userProfileRepositoryMock
                .Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(userProfile);

            _userProfileRepositoryMock
                .Setup(x => x.UpdateAsync(It.IsAny<UserProfile>()))
                .ReturnsAsync(userProfile);

            _mapperMock
                .Setup(m => m.Map<ResponseUserProfileDto>(It.IsAny<UserProfile>()))
                .Returns(responseUserProfileDto);

            //Act
            var result = await _updateUserProfileCommandHandler.Handle(updateUserProfileCommand, new CancellationToken());

            //Assert
            Assert.NotNull(result);
            Assert.Equal(updateUserProfileDto.FirstName, result.FirstName);
            Assert.Equal(updateUserProfileDto.LastName, result.LastName);
            Assert.Equal(updateUserProfileDto.ProfilePicture, result.ProfilePicture);
            Assert.Equal(updateUserProfileDto.Bio, result.Bio);
            Assert.Equal(updateUserProfileDto.DateOfBirth, result.DateOfBirth);

            _unitOfWorkMock.Verify(uow => uow.SaveAsync(), Times.Once);
            _userProfileRepositoryMock.Verify(repo => repo.UpdateAsync(It.Is<UserProfile>(up => up.FirstName == updateUserProfileDto.FirstName && up.LastName == updateUserProfileDto.LastName)), Times.Once);
            _mapperMock.Verify(mapper => mapper.Map<ResponseUserProfileDto>(It.IsAny<UserProfile>()), Times.Once);
        }
    }
}
