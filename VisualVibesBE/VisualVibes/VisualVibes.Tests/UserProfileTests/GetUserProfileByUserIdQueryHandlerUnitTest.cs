
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using VisualVibes.App.DTOs.UserProfileDtos;
using VisualVibes.App.Interfaces;
using VisualVibes.App.UserProfiles.Queries;
using VisualVibes.App.UserProfiles.QueriesHandler;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Tests.UserProfileTests
{
    public class GetUserProfileByUserIdQueryHandlerUnitTest
    {
        private readonly GetUserProfileByUserIdQueryHandler _getUserProfileByUserIdQueryHandler;
        private readonly Mock<IUserProfileRepository> _userProfileRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetUserProfileByUserIdQueryHandler>> _loggerMock;
        private readonly Mock<IMapper> _mapperMock;

        public GetUserProfileByUserIdQueryHandlerUnitTest()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _userProfileRepositoryMock = new Mock<IUserProfileRepository>();
            _loggerMock = new Mock<ILogger<GetUserProfileByUserIdQueryHandler>>();
            _mapperMock = new Mock<IMapper>();

            _unitOfWorkMock.Setup(uow => uow.UserProfileRepository).Returns(_userProfileRepositoryMock.Object);

            _getUserProfileByUserIdQueryHandler = new GetUserProfileByUserIdQueryHandler(_unitOfWorkMock.Object, _loggerMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Should_GetUserProfileByUserId_Correctly()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var user = new User 
            { 
                UserProfile = new UserProfile() 
            };

            var responseDto = new ResponseUserProfileDto();

            _userProfileRepositoryMock
                .Setup(up => up.GetUserWithProfileByIdAsync(userId))
                .ReturnsAsync(user);

            _mapperMock.Setup(mapper => mapper.Map<ResponseUserProfileDto>(user))
                .Returns(responseDto);

            // Act
            var result = await _getUserProfileByUserIdQueryHandler.Handle(new GetUserProfileByUserIdQuery(userId), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(responseDto, result);
            _mapperMock.Verify(m => m.Map<ResponseUserProfileDto>(user), Times.Once);
        }
    }
}
