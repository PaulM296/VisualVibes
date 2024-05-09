using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IUserProfileRepository : IBaseRepository<UserProfile>
    {
        Task<User> GetUserWithProfileByIdAsync(Guid userId);
    }
}
