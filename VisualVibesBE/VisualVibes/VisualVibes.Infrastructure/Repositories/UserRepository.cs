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
            var user = await _context.Users
                .Include(u => u.UserProfile)
                    .ThenInclude(i => i.Image)
                .Include(u => u.Followers)
                    .ThenInclude(f => f.Follower)
                .Include(u => u.Following)
                    .ThenInclude(f => f.Following)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                throw new EntityNotFoundException($"User with ID {id} not found.");
            }
            return user;
        }

        public async Task<AppUser> RemoveUserAsync(AppUser entity)
        {
            var entityToRemove = await _context.Set<AppUser>().FirstOrDefaultAsync(u => u.Id == entity.Id);

            if (entityToRemove == null)
            {
                throw new EntityNotFoundException($"The {nameof(AppUser)} does not exist, therefore it could not be removed.");
            }

            _context.Set<AppUser>().Remove(entityToRemove);
            await _context.SaveChangesAsync();

            return entityToRemove;
        }

        public async Task<AppUser> UpdateUserAsync(AppUser updatedUser)
        {
            var user = await _context.Set<AppUser>().FirstOrDefaultAsync(e => e.Id == updatedUser.Id);

            if (user == null)
            {
                throw new EntityNotFoundException($"The {nameof(AppUser)} has not been found, therefore it could not be removed.");
            }

            _context.Set<AppUser>().Update(updatedUser);
            await _context.SaveChangesAsync();

            return user;
        }

    }
}
