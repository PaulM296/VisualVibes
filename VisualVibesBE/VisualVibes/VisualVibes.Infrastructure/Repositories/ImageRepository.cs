using Microsoft.EntityFrameworkCore;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly VisualVibesDbContext _context;

        public ImageRepository(VisualVibesDbContext context)
        {
            _context = context;
        }

        public async Task RemoveImage(Guid id)
        {
            var image = await _context.Images.SingleOrDefaultAsync(x => x.Id == id);
            if (image != null)
            {
                _context.Images.Remove(image);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Image> GetImageById(Guid id)
        {
            return await _context.Images.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task UploadImage(Image image)
        {
            await _context.Images.AddAsync(image);
            await _context.SaveChangesAsync();
        }
    }
}
