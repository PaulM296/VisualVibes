namespace VisualVibes.App.DTOs.PostDtos
{
    public class CreatePostDto
    {
        public Guid UserId { get; set; }
        public string Caption { get; set; }
        public string Pictures { get; set; }
    }
}
