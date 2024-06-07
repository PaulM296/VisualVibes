using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.DTOs.ReactionDtos;

namespace VisualVibes.App.DTOs.PostDtos
{
    public class PostsDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Caption { get; set; }
        public string PictureUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<ResponseReactionDto> Reactions { get; set; }
        public int ReactionCount { get; set; }
        public List<ResponseCommentDto> Comments { get; set; }
        public int CommentCount { get; set; }
    }
}
