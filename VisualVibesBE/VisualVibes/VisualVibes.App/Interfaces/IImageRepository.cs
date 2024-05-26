using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IImageRepository
    {
        Task<Image> GetImageById(Guid id);
        Task UploadImage(Image image);
        Task RemoveImage(Guid id);
    }
}
