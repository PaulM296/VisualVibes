using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.App.DTOs.FeedDtos;
using VisualVibes.App.Feeds.Commands;

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
        
    }
}
