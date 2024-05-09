using MediatR;
using Microsoft.AspNetCore.Mvc;
using VisualVibes.App.Conversations.Commands;
using VisualVibes.App.Conversations.Queries;
using VisualVibes.App.DTOs.ConversationDtos;
using VisualVibes.App.DTOs.UserDtos;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ConversationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateConversation(CreateConversationDto requestConversationDto)
        {
            var response = await _mediator.Send(new CreateConversationCommand(requestConversationDto));

            return Ok(response);
        }

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetAllUserConversations(Guid id)
        {
            var conversations = await _mediator.Send(new GetAllUserConversationsQuery(id));

            return Ok(conversations);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUser(Guid id)
        {
            var response = await _mediator.Send(new RemoveConversationCommand(id));

            return Ok();
        }
    }
}
