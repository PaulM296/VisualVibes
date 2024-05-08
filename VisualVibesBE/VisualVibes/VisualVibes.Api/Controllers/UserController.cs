using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VisualVibes.App.DTOs.UserDtos;
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
        public async Task<IActionResult> CreateUser(RequestUserDto requestUserDto)
        {
            var respone = await _mediator.Send(new CreateUserCommand(requestUserDto));

            return Ok(respone);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(Guid id)
        {
            var users = await _mediator.Send(new GetUserByIdQuery(id));

            return Ok(users);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, RequestUserDto requestUserDto)
        {
            var updatedUser = await _mediator.Send(new UpdateUserCommand(id, requestUserDto));

            return Ok(updatedUser);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveUser(Guid id)
        {
            var response = await _mediator.Send(new RemoveUserCommand(id));

            return Ok(response);
        }
    }
}
