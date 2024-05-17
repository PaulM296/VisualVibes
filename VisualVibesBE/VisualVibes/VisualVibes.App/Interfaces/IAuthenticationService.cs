using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;

namespace VisualVibes.App.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AppUser> Register(AppUser newUser, UserProfile newUserProfile, string password);
        Task<AppUser> LoginUser(string Email, string Password);
    }
}
