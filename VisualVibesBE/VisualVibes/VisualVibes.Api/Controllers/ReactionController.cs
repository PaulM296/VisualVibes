using MediatR;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.App.Comments.Queries;
using VisualVibes.App.DTOs.ReactionDtos;
using VisualVibes.App.Reactions.Commands;
using VisualVibes.App.Reactions.Queries;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReactionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ReactionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReaction(CreateReactionDto createReactionDto)
        {
            var createReaction = await _mediator.Send(new CreateReactionCommand(createReactionDto));

            return Ok(createReaction);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveReaction(Guid id)
        {
            var removeReaction = await _mediator.Send(new RemoveReactionCommand(id));

            return Ok(removeReaction);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReaction(Guid id, UpdateReactionDto updateReactionDto)
        {
            var updateReaction = await _mediator.Send(new UpdateReactionCommand(id, updateReactionDto));

            return Ok(updateReaction);
        }

        [HttpGet("post/{id}")]
        public async Task<IActionResult> GetAllPostReactions(Guid id)
        {
            var postReactions = await _mediator.Send(new GetAllPostReactionsQuery(id));

            return Ok(postReactions);
        }

    }
}
