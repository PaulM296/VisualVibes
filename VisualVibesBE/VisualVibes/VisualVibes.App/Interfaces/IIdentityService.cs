using System.Security.Claims;
using VisualVibes.Domain.Models;

namespace VisualVibes.App.Interfaces
{
    public interface IIdentityService
    {
        string CreateSecurityToken(ClaimsIdentity identity);
        ClaimsIdentity CreateClaimsIdentity(AppUser newUser);
    }
}
