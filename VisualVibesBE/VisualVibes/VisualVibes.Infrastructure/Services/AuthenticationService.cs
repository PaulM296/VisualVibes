using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using VisualVibes.App.Interfaces;
using VisualVibes.Domain.Models;
using VisualVibes.Domain.Models.BaseEntity;
using VisualVibes.Infrastructure.Exceptions;

namespace VisualVibes.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly VisualVibesDbContext _context;

        public AuthenticationService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager, VisualVibesDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<AppUser> Register(AppUser newUser, UserProfile newUserProfile, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(newUser.Email);

            if(existingUser != null)
            {
                throw new EmailAlreadyExistsException("This email is already in use.");
            }

            var identityUser = new AppUser
            {
                Email = newUser.Email,
                UserName = newUser.UserName,
                Role = newUser.Role
            };

            var createdIdentity = await _userManager.CreateAsync(identityUser, password);

            var foundRole = await _roleManager.FindByNameAsync(newUser.Role.ToString());
            if(foundRole == null)
            {
                var newRole = new IdentityRole(newUser.Role.ToString());
                await _roleManager.CreateAsync(newRole);
            }

            await _userManager.AddToRoleAsync(identityUser, newUser.Role.ToString());

            if (!createdIdentity.Succeeded)
            {
                throw new Exception("User creation failed!");
            }

            var userProfile = new UserProfile
            {
                FirstName = newUserProfile.FirstName,
                LastName = newUserProfile.LastName,
                UserId = identityUser.Id,
                ProfilePicture = newUserProfile.ProfilePicture,
                Bio = newUserProfile.Bio,
                DateOfBirth = newUserProfile.DateOfBirth
            };

            _context.UserProfiles.Add(userProfile);
            await _context.SaveChangesAsync();

            return identityUser;
        }
        public Task<AppUser> LoginUser(string Email, string Password)
        {
            throw new NotImplementedException();
        }
    }
}
