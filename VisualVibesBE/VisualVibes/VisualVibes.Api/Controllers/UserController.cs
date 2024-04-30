using Microsoft.AspNetCore.Mvc;
using VisualVibes.App.DTOs;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateUser(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(userDto);
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = new List<UserDto>
            {
                new UserDto
                {
                    Id = Guid.NewGuid(),
                    Username = "Waganaha",
                    Password = "456123"
                },

                new UserDto
                {
                    Id = Guid.NewGuid(),
                    Username = "Paulinho",
                    Password = "123456789",
                },

                new UserDto
                {
                    Id = Guid.NewGuid(),
                    Username = "LukeX19",
                    Password = "123456",
                }
            };
            return Ok(users);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateUser(UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(userDto);
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveUser(Guid Id)
        {
            return Ok();
        }
    }
}
