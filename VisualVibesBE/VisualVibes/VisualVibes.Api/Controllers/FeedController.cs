using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.Api.Extensions;
using VisualVibes.App.DTOs.FeedDtos;
using VisualVibes.App.Feeds.Commands;
using VisualVibes.App.Feeds.Queries;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/feeds")]
    [Authorize]
    public class FeedController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FeedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateFeed(CreateFeedDto createFeedDto)
        {
            var createFeed = await _mediator.Send(new CreateFeedCommand(createFeedDto));

            return Ok(createFeed);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserFeed()
        {
            var userId = HttpContext.GetUserIdClaimValue();

            var query = new GetUserFeedByUserIdQuery(userId);
            var responseFeedDto = await _mediator.Send(query);

            if (responseFeedDto == null)
            {
                return NotFound($"Feed for user {userId} not found.");
            }

            return Ok(responseFeedDto);
        }

    }
}
