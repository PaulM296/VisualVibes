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
    }
}
