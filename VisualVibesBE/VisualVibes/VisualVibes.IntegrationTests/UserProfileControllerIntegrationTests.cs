using VisualVibes.Api.Controllers;
using VisualVibes.Infrastructure.Repositories;
using VisualVibes.Infrastructure;
using VisualVibes.IntegrationTests.Helpers;
using VisualVibes.App.DTOs.UserProfileDtos;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace VisualVibes.IntegrationTests
{
    public class UserProfileControllerIntegrationTests
    {
        [Fact]
        public async Task UserProfileController_CreateUserProfile_ShouldCreateUserProfileInDbCorrectly()
        {
            //Arrange
            using var contextBuilder = new DataContextBuilder(nameof(UserProfileController_CreateUserProfile_ShouldCreateUserProfileInDbCorrectly));
            var user = contextBuilder.SeedUser();
            var dbContext = contextBuilder.GetDbContext();

            var createUserProfileDto = new CreateUserProfileDto
            {
                UserId = user.Id,
                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com",
                ProfilePicture = "picture1.jpg",
                Bio = "Bio of John Doe",
                DateOfBirth = new DateTime(2000, 10, 18)
            };

            var userRepository = new UserRepository(dbContext);
            var userProfileRepository = new UserProfileRepository(dbContext);
            var userFollowerRepository = new UserFollowerRepository(dbContext);
            var reactionRepository = new ReactionRepository(dbContext);
            var postRepository = new PostRepository(dbContext);
            var messageRepository = new MessageRepository(dbContext);
            var feedRepository = new FeedRepository(dbContext);
            var feedPostRepository = new FeedPostRepository(dbContext);
            var conversationRepository = new ConversationRepository(dbContext);
            var commentRepository = new CommentRepository(dbContext);


            var unitOfWork = new UnitOfWork(dbContext, commentRepository, conversationRepository, feedRepository, messageRepository,
                postRepository, reactionRepository, userRepository, userProfileRepository, userFollowerRepository, feedPostRepository);

            var mediator = TestHelpers.CreateMediator(unitOfWork);

            var controller = new UserProfileController(mediator);

            //Act
            var requestResult = await controller.CreateUserProfile(createUserProfileDto);

            //Assert
            var result = requestResult as CreatedResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);

            var responseUserProfileDto = result.Value as ResponseUserProfileDto;
            Assert.NotNull(responseUserProfileDto);
            Assert.Equal(createUserProfileDto.FirstName, responseUserProfileDto.FirstName);
            Assert.Equal(createUserProfileDto.LastName, responseUserProfileDto.LastName);
            Assert.Equal(createUserProfileDto.Email, responseUserProfileDto.Email);
            Assert.Equal(createUserProfileDto.ProfilePicture, responseUserProfileDto.ProfilePicture);
            Assert.Equal(createUserProfileDto.Bio, responseUserProfileDto.Bio);
            Assert.Equal(createUserProfileDto.DateOfBirth, responseUserProfileDto.DateOfBirth);
        }

        [Fact]
        public async Task UserProfileController_UpdateUserProfile_ShouldUpdateUserProfileInDbCorrectly()
        {
            //Arrange
            using var contextBuilder = new DataContextBuilder(nameof(UserProfileController_UpdateUserProfile_ShouldUpdateUserProfileInDbCorrectly));
            var user = contextBuilder.SeedUser();
            var dbContext = contextBuilder.GetDbContext();
            var userProfile = contextBuilder.SeedUserProfile(user.Id);

            var updateUserProfileDto = new UpdateUserProfileDto
            {
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                Email = "updatedEmail",
                ProfilePicture = "updatedProfilePicture.png",
                Bio = userProfile.Bio,
                DateOfBirth = userProfile.DateOfBirth
            };

            var userRepository = new UserRepository(dbContext);
            var userProfileRepository = new UserProfileRepository(dbContext);
            var userFollowerRepository = new UserFollowerRepository(dbContext);
            var reactionRepository = new ReactionRepository(dbContext);
            var postRepository = new PostRepository(dbContext);
            var messageRepository = new MessageRepository(dbContext);
            var feedRepository = new FeedRepository(dbContext);
            var feedPostRepository = new FeedPostRepository(dbContext);
            var conversationRepository = new ConversationRepository(dbContext);
            var commentRepository = new CommentRepository(dbContext);


            var unitOfWork = new UnitOfWork(dbContext, commentRepository, conversationRepository, feedRepository, messageRepository,
                postRepository, reactionRepository, userRepository, userProfileRepository, userFollowerRepository, feedPostRepository);

            var mediator = TestHelpers.CreateMediator(unitOfWork);

            var controller = new UserProfileController(mediator);

            //Act
            var requestResult = await controller.UpdateUserProfile(userProfile.Id, updateUserProfileDto);

            //Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var responseUserProfileDto = result.Value as ResponseUserProfileDto;
            Assert.NotNull(responseUserProfileDto);
            Assert.Equal(updateUserProfileDto.FirstName, responseUserProfileDto.FirstName);
            Assert.Equal(updateUserProfileDto.LastName, responseUserProfileDto.LastName);
            Assert.Equal(updateUserProfileDto.Email, responseUserProfileDto.Email);
            Assert.Equal(updateUserProfileDto.ProfilePicture, responseUserProfileDto.ProfilePicture);
            Assert.Equal(updateUserProfileDto.Bio, responseUserProfileDto.Bio);
            Assert.Equal(updateUserProfileDto.DateOfBirth, responseUserProfileDto.DateOfBirth);
        }

        [Fact]
        public async Task UserProfileController_GetUserProfileByUserId_ShouldGetUserProfileByUserIdFromDbCorrectly()
        {
            //Arrange
            using var contextBuilder = new DataContextBuilder(nameof(UserProfileController_GetUserProfileByUserId_ShouldGetUserProfileByUserIdFromDbCorrectly));
            var user = contextBuilder.SeedUser();
            var userProfile = contextBuilder.SeedUserProfile(user.Id);
            var dbContext = contextBuilder.GetDbContext();

            var userRepository = new UserRepository(dbContext);
            var userProfileRepository = new UserProfileRepository(dbContext);
            var userFollowerRepository = new UserFollowerRepository(dbContext);
            var reactionRepository = new ReactionRepository(dbContext);
            var postRepository = new PostRepository(dbContext);
            var messageRepository = new MessageRepository(dbContext);
            var feedRepository = new FeedRepository(dbContext);
            var feedPostRepository = new FeedPostRepository(dbContext);
            var conversationRepository = new ConversationRepository(dbContext);
            var commentRepository = new CommentRepository(dbContext);


            var unitOfWork = new UnitOfWork(dbContext, commentRepository, conversationRepository, feedRepository, messageRepository,
                postRepository, reactionRepository, userRepository, userProfileRepository, userFollowerRepository, feedPostRepository);

            var mediator = TestHelpers.CreateMediator(unitOfWork);

            var controller = new UserProfileController(mediator);

            //Act
            var requestResult = await controller.GetUserProfileByUserId(userProfile.UserId);

            //Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var responseUserProfileDto = result.Value as ResponseUserProfileDto;
            Assert.NotNull(responseUserProfileDto);
            Assert.Equal(userProfile.FirstName, responseUserProfileDto.FirstName);
            Assert.Equal(userProfile.LastName, responseUserProfileDto.LastName);
            Assert.Equal(userProfile.Email, responseUserProfileDto.Email);
            Assert.Equal(userProfile.ProfilePicture, responseUserProfileDto.ProfilePicture);
            Assert.Equal(userProfile.Bio, responseUserProfileDto.Bio);
            Assert.Equal(userProfile.DateOfBirth, responseUserProfileDto.DateOfBirth);
        }
    }
}
