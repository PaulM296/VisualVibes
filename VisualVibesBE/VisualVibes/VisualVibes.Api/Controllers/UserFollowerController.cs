using MediatR;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.App.UserFollowers.Commands;
using VisualVibes.App.UserFollowers.Queries;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserFollowerController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserFollowerController(IMediator mediator)
        {
            _mediator = mediator;
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
