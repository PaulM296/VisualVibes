using VisualVibes.App.DTOs.CommentDtos;
using VisualVibes.App.DTOs.ReactionDtos;

namespace VisualVibes.App.DTOs.FeedDtos
{
    public class FeedPostDto
    {
        public Guid PostId {  get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string? Caption { get; set; }
        public string? Pictures { get; set; }
        public DateTime CreatedAt { get; set; }
        public int ReactionCount {  get; set; }
        public int CommentCount { get; set; }
        public Guid PostImageId { get; set; }
        public Guid UserProfileImageId { get; set; }
        public bool isModerated { get; set; }
        public ICollection<ResponseCommentDto> Comments { get; set; }
        public ICollection<ResponseReactionDto> Reactions { get; set; }
    }
}
