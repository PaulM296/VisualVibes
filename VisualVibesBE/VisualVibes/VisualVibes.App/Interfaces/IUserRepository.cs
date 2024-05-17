using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IUserRepository
    {
        void CreateUser(UserProfile userProfile);
        void ChangePassword(int id, string newPassword);
        Task<AppUser> GetUserByIdAsync(string id);
    }
}
