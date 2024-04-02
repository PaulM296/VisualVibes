using VisualVibes.App;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(FileSystemLogger logger) : base(logger)
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
    }
}
