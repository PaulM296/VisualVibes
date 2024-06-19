using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.DTOs.ReactionDtos;

namespace VisualVibes.App.DTOs.PostDtos
{
    public class JoinedResponsePostDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Caption { get; set; }
        public string Pictures { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid ImageId { get; set; }
        public bool isModerated { get; set; }
        public ICollection<ResponseCommentDto> Comments { get; set; }
        public ICollection<ResponseReactionDto> Reactions { get; set; }
    }
}
