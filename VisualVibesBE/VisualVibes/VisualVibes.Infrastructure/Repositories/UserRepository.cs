﻿using Microsoft.EntityFrameworkCore;
using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;
using VisualVibes.Infrastructure.Exceptions;

namespace VisualVibes.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(VisualVibesDbContext context) : base(context)
        {

        }
        public void ChangePassword(int id, string newPassword)
        {
            throw new NotImplementedException();
        }

        public void CreateUser(UserProfile userProfile)
        {
            throw new NotImplementedException();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
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
