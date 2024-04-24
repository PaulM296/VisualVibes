﻿using Microsoft.EntityFrameworkCore;
using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Repositories
{
    public class UserProfileRepository : BaseRepository<UserProfile>, IUserProfileRepository
    {
        public UserProfileRepository(VisualVibesDbContext context, FileSystemLogger logger) : base(context, logger)
        {

        }
    }
}
