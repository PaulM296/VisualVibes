using Microsoft.AspNetCore.Mvc;
using VisualVibes.App.DTOs;

namespace VisualVibes.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreatePost(PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(postDto);
        }

        [HttpGet]
        public IActionResult GetAllPosts()
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

            var posts = new List<PostDto>
            {
                new PostDto
                {
                    Id = Guid.NewGuid(),
                    UserId = user1.Id,
                    Caption = "This is a new post",
                    Pictures = "picture1",
                    CreatedAt = DateTime.UtcNow
                },
                new PostDto
                {
                    Id = Guid.NewGuid(),
                    UserId = user2.Id,
                    Caption = "This is a new new post",
                    Pictures = "picture1, picture2",
                    CreatedAt = DateTime.UtcNow
                }
            };
            return Ok(posts);
        }


        [HttpPut("{id}")]
        public IActionResult UpdatePost(PostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(postDto);
        }

        [HttpDelete("{id}")]
        public IActionResult RemovePost(Guid Id)
        {
            return Ok();
        }
    }
}
