using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VisualVibes.Api.Controllers;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.App.DTOs.UserFollowerDtos;
using VisualVibes.App.UserFollowers.Commands;
using VisualVibes.Infrastructure;
using VisualVibes.Infrastructure.Repositories;
using VisualVibes.IntegrationTests.Helpers;

namespace VisualVibes.IntegrationTests
{
    public class UserControllerIntegrationTests
    {
        [Fact]
        public async Task UserController_CreateUser_ShouldCreateUserInDbCorrectly()
        {
            //Arrange
            var createUserDto = new CreateUserDto
            {
                Username = "TestUser",
                Password = "passwordTest",
            };

            using var contextBuilder = new DataContextBuilder(nameof(UserController_CreateUser_ShouldCreateUserInDbCorrectly));
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

            var controller = new UserController(mediator);

            //Act
            var requestResult = await controller.CreateUser(createUserDto);

            //Assert
            var result = requestResult as CreatedAtActionResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.Created, result.StatusCode);

            var responseUserDto = result.Value as ResponseUserDto;
            Assert.NotNull(responseUserDto);
            Assert.Equal(createUserDto.Username, responseUserDto.Username);
            Assert.Equal(createUserDto.Password, responseUserDto.Password);

            var createdUser = await dbContext.Users.FindAsync(responseUserDto.Id);
            Assert.NotNull(createdUser);
            Assert.Equal(createUserDto.Username, createdUser.Username);
            Assert.Equal(createUserDto.Password, createdUser.Password);

        }

        [Fact]
        public async Task UserController_UpdateUser_ShouldUpdateUserInDbCorrectly()
        {
            //Arrange
            using var contextBuilder = new DataContextBuilder(nameof(UserController_UpdateUser_ShouldUpdateUserInDbCorrectly));
            var user = contextBuilder.SeedUser();
            var dbContext = contextBuilder.GetDbContext();

            var updateUserDto = new UpdateUserDto
            {
                Username = "UpdatedUsernameTest"
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

            var controller = new UserController(mediator);

            //Act
            var requestResult = await controller.UpdateUser(user.Id, updateUserDto);

            //Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var responseUserDto = result.Value as ResponseUserDto;
            Assert.NotNull(responseUserDto);
            Assert.Equal(user.Id, responseUserDto.Id);
            Assert.Equal(updateUserDto.Username, responseUserDto.Username);
        }

        [Fact]
        public async Task UserController_RemoveUserById_ShouldRemoveUserInDbCorrectly()
        {
            //Arrange
            using var contextBuilder = new DataContextBuilder(nameof(UserController_RemoveUserById_ShouldRemoveUserInDbCorrectly));
            var user = contextBuilder.SeedUser();
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

            var controller = new UserController(mediator);

            //Act
            var requestResult = await controller.RemoveUser(user.Id);

            //Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var removedUser = await dbContext.Users.FindAsync(user.Id);
            Assert.Null(removedUser);
        }

        [Fact]
        public async Task UserController_GetUserById_ShouldGetUserByFromDbCorrectly()
        {
            //Arrange
            using var contextBuilder = new DataContextBuilder(nameof(UserController_RemoveUserById_ShouldRemoveUserInDbCorrectly));
            var user = contextBuilder.SeedUser();
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

            var controller = new UserController(mediator);

            //Act
            var requestResult = await controller.GetUserById(user.Id);

            //Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var responseUserDto = result.Value as ResponseUserDto;
            Assert.NotNull(responseUserDto);
            Assert.Equal(user.Id, responseUserDto.Id);
            Assert.Equal(user.Username, responseUserDto.Username);
        }

        [Fact]
        public async Task UserController_FollowUser_ShouldAddFollowerCorrectly()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder(nameof(UserController_FollowUser_ShouldAddFollowerCorrectly));
            var users = contextBuilder.SeedUsers(2);
            var follower = users[0];
            var following = users[1];
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

            var controller = new UserController(mediator);

            var followUserCommand = new FollowUserCommand(follower.Id, following.Id);


            // Act
            var requestResult = await controller.FollowUser(followUserCommand);

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var userFollower = await dbContext.UserFollower.FirstOrDefaultAsync(uf => uf.FollowerId == follower.Id && uf.FollowingId == following.Id);
            Assert.NotNull(userFollower);
            Assert.Equal(follower.Id, userFollower.FollowerId);
            Assert.Equal(following.Id, userFollower.FollowingId);
        }

        [Fact]
        public async Task UserController_UnfollowUser_ShouldRemoveFollowerCorrectly()
        {
            // Arrange
            using var contextBuilder = new DataContextBuilder(nameof(UserController_UnfollowUser_ShouldRemoveFollowerCorrectly));
            var users = contextBuilder.SeedUsers(2);
            var follower = users[0];
            var following = users[1];
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

            var controller = new UserController(mediator);

            var followUserCommand = new FollowUserCommand(follower.Id, following.Id);
            var unfollowUserCommand = new UnfollowUserCommand(follower.Id, following.Id);

            //Act
            var followRequestResult = await controller.FollowUser(followUserCommand);
            var unfollowRequestResult = await controller.UnfollowUser(unfollowUserCommand);

            //Assert
            var result = unfollowRequestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var removedUserFollower = await dbContext.UserFollower.FirstOrDefaultAsync(uf => uf.FollowerId == follower.Id && uf.FollowingId == following.Id);
            Assert.Null(removedUserFollower);
        }

        [Fact]
        public async Task UserController_GetUserFollowers_ShouldReturnFollowersCorrectly()
        {
            //Arrange
            using var contextBuilder = new DataContextBuilder(nameof(UserController_UnfollowUser_ShouldRemoveFollowerCorrectly));
            var users = contextBuilder.SeedUsers(3);
            var user = users[0];
            var follower1 = users[1];
            var follower2 = users[2];
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

            var controller = new UserController(mediator);

            var followUserCommand1 = new FollowUserCommand(follower1.Id, user.Id);
            var followUserCommand2 = new FollowUserCommand(follower2.Id, user.Id);

            //Act
            await controller.FollowUser(followUserCommand1);
            await controller.FollowUser(followUserCommand2);

            var requestResult = await controller.GetUserFollowers(user.Id);

            // Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var userFollowers = result.Value as IEnumerable<UserFollowerDto>;
            Assert.NotNull(userFollowers);
            Assert.Contains(userFollowers, uf => uf.FollowerId == follower1.Id);
            Assert.Contains(userFollowers, uf => uf.FollowerId == follower2.Id);
        }

        [Fact]
        public async Task UserController_GetUserFollowing_ShouldReturnFollowingCorrectly()
        {
            //Arrange
            using var contextBuilder = new DataContextBuilder(nameof(UserController_GetUserFollowing_ShouldReturnFollowingCorrectly));
            var users = contextBuilder.SeedUsers(3);
            var user = users[0];
            var following1 = users[1];
            var following2 = users[2];
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

            var controller = new UserController(mediator);

            var followUserCommand1 = new FollowUserCommand(user.Id, following1.Id);
            var followUserCommand2 = new FollowUserCommand(user.Id, following2.Id);

            await controller.FollowUser(followUserCommand1);
            await controller.FollowUser(followUserCommand2);

            //Act
            var requestResult = await controller.GetUserFollowing(user.Id);

            //Assert
            var result = requestResult as OkObjectResult;
            Assert.NotNull(result);
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);

            var userFollowing = result.Value as IEnumerable<UserFollowerDto>;
            Assert.NotNull(userFollowing);
            Assert.Contains(userFollowing, uf => uf.FollowingId == following1.Id);
            Assert.Contains(userFollowing, uf => uf.FollowingId == following2.Id);
        }
    }
}
