namespace VisualVibes.App.DTOs.PostDtos
{
    public class UpdatePostDto
    {
        public Guid UserId { get; set; }
        public string Caption { get; set; }
        public string Pictures { get; set; }
    }
}
