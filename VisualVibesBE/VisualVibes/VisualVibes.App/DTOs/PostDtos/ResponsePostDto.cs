using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.DTOs.PostDtos
{
    public class ResponsePostDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Caption { get; set; }
        public string Pictures { get; set; }
        public DateTime CreatedAt { get; set; }

        //public static ResponsePostDto FromPost(Post post)
        //{
        //    return new ResponsePostDto
        //    {
        //        Id = post.Id,
        //        UserId = post.UserId,
        //        Caption = post.Caption,
        //        Pictures = post.Pictures,
        //        CreatedAt = post.CreatedAt
        //    };
        //}
    }
}
