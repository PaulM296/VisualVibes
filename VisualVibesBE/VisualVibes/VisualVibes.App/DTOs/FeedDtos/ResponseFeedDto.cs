namespace VisualVibes.App.DTOs.FeedDtos
{
    public class ResponseFeedDto
    {
        public string UserID { get; set; }
        public List<FeedPostDto> Posts { get; set; } = new List<FeedPostDto>();
    }
}
