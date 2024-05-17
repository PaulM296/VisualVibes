using Microsoft.EntityFrameworkCore;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;
using VisualVibes.Infrastructure.Exceptions;

namespace VisualVibes.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly VisualVibesDbContext _context;
        public UserRepository(VisualVibesDbContext context)
        {
            _context = context;
        }
        public void ChangePassword(int id, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(UserProfile userProfile)
        {
            throw new NotImplementedException();
        }

        public async Task<AppUser> GetUserByIdAsync(string id)
        {
            var entity = await _context.Users
                .Include(u => u.Followers)
                    .ThenInclude(f => f.Follower)
                .Include(u => u.Following)
                    .ThenInclude(f => f.Following)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (entity == null)
            {
                throw new EntityNotFoundException($"User with ID {id} not found.");
            }
            return entity;
        }

    }
}
