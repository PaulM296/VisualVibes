using Microsoft.EntityFrameworkCore;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Repositories
{
    public class UserProfileRepository : BaseRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(VisualVibesDbContext context) : base(context)
        {

        }

        public async Task<UserProfile> GetUserProfileByUserId(Guid userId)
        {
            return await _context.UserProfiles.FirstOrDefaultAsync(up => up.UserId == userId);
        }
    }
}
