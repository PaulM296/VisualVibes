using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        void CreateUser(UserProfile userProfile);
        void ChangePassword(int id, string newPassword);
    }
}
