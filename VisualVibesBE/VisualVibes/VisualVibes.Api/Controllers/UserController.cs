using Azure;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisualVibes.App.DTOs.UserDtos;
using VisualVibes.App.UserFollowers.Commands;
using VisualVibes.App.UserFollowers.Queries;
using VisualVibes.App.Users.Commands;
using VisualVibes.App.Users.Queries;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
        {
            var response = await _mediator.Send(new CreateUserCommand(createUserDto));

            return CreatedAtAction(nameof(GetUserById), new { id = response.Id }, response);
            //return Created(string.Empty, response);
        }

        [HttpPost("follow")]
        public async Task<IActionResult> FollowUser([FromBody] FollowUserCommand command)
        {
            var follower = await _mediator.Send(command);

            return Ok(follower);
        }

        [HttpPost("unfollow")]
        public async Task<IActionResult> UnfollowUser([FromBody] UnfollowUserCommand command)
        {
            var response = await _mediator.Send(command);

            return Ok(response);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UpdateUserDto updateUserDto)
        {
            var updatedUser = await _mediator.Send(new UpdateUserCommand(id, updateUserDto));

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUser(Guid id)
        {
            var response = await _mediator.Send(new RemoveUserCommand(id));

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var users = await _mediator.Send(new GetUserByIdQuery(id));

            return Ok(users);
        }

        [HttpGet("{userId}/followers")]
        public async Task<IActionResult> GetUserFollowers(Guid userId)
        {
            var userFollowers = await _mediator.Send(new GetUserFollowersByIdQuery(userId));

            return Ok(userFollowers);
        }

        [HttpGet("{userId}/following")]
        public async Task<IActionResult> GetUserFollowing(Guid userId)
        {
            var userFollowers = await _mediator.Send(new GetUserFollowingByIdQuery(userId));

            return Ok(userFollowers);
        }

    }
}