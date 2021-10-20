using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace invMed.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly ILogger<IAdminService> _logger;

        public AdminService(ApplicationDbContext db, UserManager<AspNetUser> userManager, ILogger<IAdminService> logger)
        {
            _db = db;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<List<UserView>> GetUsers()
        {
            return await (from u in _db.Users
                          where u.IsActive == true
                          join ur in _db.UserRoles on u.Id equals ur.UserId
                          join r in _db.Roles on ur.RoleId equals r.Id
                          orderby u.Name
                          select new UserView
                          {
                              Id = u.Id,
                              Name = u.Name,
                              Surname = u.Surname,
                              UserName = u.UserName,
                              Email = u.Email,
                              Role = r.Name
                          })
            .ToListAsync();
        }

        public async Task<AspNetUser> GetUserById(string id)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> UpdateUser(AspNetUser user)
        {
            try
            {
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeactivateAccount(AspNetUser user)
        {
            user.IsActive = false;
            try
            {
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddUser(AspNetUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            return result.Succeeded;
        }

        public async Task<bool> AddToRole(AspNetUser user, string role)
        {
            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<bool> RemoveFromRole(AspNetUser user, string role)
        {
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            return result.Succeeded;
        }

        public async Task<List<UserView>> GetDeactivatedUsers()
        {
            return await (from u in _db.Users
                          where u.IsActive == false
                          join ur in _db.UserRoles on u.Id equals ur.UserId
                          join r in _db.Roles on ur.RoleId equals r.Id
                          orderby u.Name
                          select new UserView
                          {
                              Id = u.Id,
                              Name = u.Name,
                              Surname = u.Surname,
                              UserName = u.UserName,
                              Email = u.Email,
                              Role = r.Name
                          })
            .ToListAsync();
        }

        public async Task<bool> ActivateAccount(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                user.IsActive = true;
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetPassword(AspNetUser user, string newPassword)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            return result.Succeeded;
        }

        public async Task<EditAccountInput> GetEditUserInputById(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            return new EditAccountInput() { Id = user.Id, Name = user.Name, Surname = user.Surname, Email = user.Email, UserName = user.UserName };
        }

        public async Task<bool> UpdateUserInput(EditAccountInput input)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == input.Id);
            user.Name = input.Name;
            user.Surname = input.Surname;
            user.Email = input.Email;
            user.UserName = input.UserName;
            try
            {
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch
            {
                return false;
            }
        }

    }
}
