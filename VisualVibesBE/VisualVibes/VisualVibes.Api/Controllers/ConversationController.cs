using Microsoft.AspNetCore.Mvc;
using VisualVibes.App.DTOs;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConversationController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateConversation(ConversationDto conversationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(conversationDto);
        }

        [HttpGet]
        public IActionResult GetAllUserConversations()
        {
            var user1 = new UserDto
            {
                Id = Guid.NewGuid(),
                Username = "Waganaha",
                Password = "456123"
            };

            var user2 = new UserDto
            {
                Id = Guid.NewGuid(),
                Username = "Paulinho",
                Password = "123456789",
            };

            var user3 = new UserDto
            {
                Id = Guid.NewGuid(),
                Username = "LukeX19",
                Password = "123456789",
            };

            var conversations = new List<ConversationDto>
            {
                new ConversationDto
                {
                    Id = Guid.NewGuid(),
                    FirstParticipantId = user1.Id,
                    SecondParticipantId = user2.Id
                },
                new ConversationDto
                {
                    Id = Guid.NewGuid(),
                    FirstParticipantId = user1.Id,
                    SecondParticipantId = user3.Id
                },
                new ConversationDto
                {
                    Id = Guid.NewGuid(),
                    FirstParticipantId = user2.Id,
                    SecondParticipantId = user3.Id
                },
            };

            return Ok(conversations);
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveUser(Guid Id)
        {
            return Ok();
        }
    }
}
