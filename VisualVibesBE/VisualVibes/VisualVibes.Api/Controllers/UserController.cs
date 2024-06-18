using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.Api.Extensions;
using VisualVibes.Api.Models;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.App.UserFollowers.Commands;
using VisualVibes.App.UserFollowers.Queries;
using VisualVibes.App.Users.Commands;
using VisualVibes.App.Users.Queries;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UserController> _logger;
        public UserController(IMediator mediator, ILogger<UserController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterUser([FromForm] RegisterUser registerUserDto)
        {
            var response = await _mediator.Send(new RegisterUserCommand(registerUserDto));

            var authenticationResult = new AuthenticationResult(response);

            return Ok(authenticationResult);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginUser(LoginDto loginUserDto)
        {
            var response = await _mediator.Send(new LoginUserCommand(loginUserDto));

            var authenticationResult = new AuthenticationResult(response);

            return Ok(authenticationResult);
        }

        [HttpPost("follow")]
        [Authorize]
        public async Task<IActionResult> FollowUser(string followingId)
        {
            var followerId = HttpContext.GetUserIdClaimValue();

            var command = new FollowUserCommand(followerId, followingId);

            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpPost("unfollow")]
        [Authorize]
        public async Task<IActionResult> UnfollowUser(string followingId)
        {
            var followerId = HttpContext.GetUserIdClaimValue();

            var command = new UnfollowUserCommand(followerId, followingId);
            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpPut("{userId}/block")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockUser(string userId)
        {
            var response = await _mediator.Send(new BlockUserCommand(userId));
            return Ok(response);
        }

        [HttpPut("{userId}/unblock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UnblockUser(string userId)
        {
            var response = await _mediator.Send(new UnblockUserCommand(userId));
            return Ok(response);
        }

        [HttpPut("{userId}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto updateUserDto)
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var updatedUser = await _mediator.Send(new UpdateUserCommand(userId, updateUserDto));

            return Ok(updatedUser);
        }

        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> RemoveUser()
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var response = await _mediator.Send(new RemoveUserCommand(userId));

            return Ok(response);
        }

        [HttpGet("logged-user")]
        [Authorize]
        public async Task<IActionResult> GetLoggedUserById()
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var users = await _mediator.Send(new GetUserByIdQuery(userId));

            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(string id)
        {
            var users = await _mediator.Send(new GetUserByIdQuery(id));

            return Ok(users);
        }

        [HttpGet("{userId}/followers")]
        [Authorize]
        public async Task<IActionResult> GetUserFollowers(string userId)
        {
            var userFollowers = await _mediator.Send(new GetUserFollowersByIdQuery(userId));

            return Ok(userFollowers);
        }

        [HttpGet("{userId}/following")]
        [Authorize]
        public async Task<IActionResult> GetUserFollowing(string userId)
        {
            var userFollowers = await _mediator.Send(new GetUserFollowingByIdQuery(userId));

            return Ok(userFollowers);
        }

        [HttpGet("search")]
        [Authorize]
        public async Task<IActionResult> SearchUsers([FromQuery] string query)
        {
            var users = await _mediator.Send(new SearchUsersQuery(query));
            return Ok(users);
        }

        [HttpGet("{userId}/is-following")]
        [Authorize]
        public async Task<IActionResult> IsFollowingUser(string userId)
        {
            var followerId = HttpContext.GetUserIdClaimValue();
            var isFollowing = await _mediator.Send(new IsFollowingUserQuery(followerId, userId));
            return Ok(isFollowing);
        }

    }
}