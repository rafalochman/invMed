using System;
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
            var users = await (from user in _db.Users
                          where user.IsActive == true
                          join userRole in _db.UserRoles on user.Id equals userRole.UserId
                          join role in _db.Roles on userRole.RoleId equals role.Id
                          orderby user.Name
                          select new UserView(user, role.Name))
            .ToListAsync();
            return users;
        }

        public async Task<UserView> GetUserById(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user is null)
            {
                _logger.LogError("Get user error - user not found.");
                return new UserView();
            }
            var userView = new UserView(user);
            return userView;
        }

        public async Task<bool> DeactivateAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            try
            {
                user.IsActive = false;
                _db.Users.Update(user);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Dectivate account error.");
                return false;
            }
        }

        public async Task<bool> AddUser(CreateAccountInput input)
        {
            var user = new AspNetUser { Name = input.Name, Surname = input.Surname, UserName = input.Email, Email = input.Email, IsActive = true };
            var result = await _userManager.CreateAsync(user, input.Password);
            if (!result.Succeeded)
            {
                _logger.LogError("Create user error.");
            }
            return result.Succeeded;
        }

        public async Task<bool> AddToRole(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                _logger.LogError("Add user to role error - user not found.");
                return false;
            }
            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
            {
                _logger.LogError("Add user to role error.");
            }
            return result.Succeeded;
        }

        public async Task<bool> AddToRole(CreateAccountInput input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user is null)
            {
                _logger.LogError("Add user to role error - user not found.");
                return false;
            }
            var result = await _userManager.AddToRoleAsync(user, input.Role);
            if (!result.Succeeded)
            {
                _logger.LogError("Add user to role error.");
            }
            return result.Succeeded;
        }

        public async Task<bool> RemoveFromRole(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                _logger.LogError("Remove user from role error - user not found.");
                return false;
            }
            var result = await _userManager.RemoveFromRoleAsync(user, role);
            if (!result.Succeeded)
            {
                _logger.LogError("Remove user from role error.");
            }
            return result.Succeeded;
        }

        public async Task<List<UserView>> GetDeactivatedUsers()
        {
            var users = await (from user in _db.Users
                          where user.IsActive == false
                          join userRole in _db.UserRoles on user.Id equals userRole.UserId
                          join role in _db.Roles on userRole.RoleId equals role.Id
                          orderby user.Name
                          select new UserView(user, role.Name))
            .ToListAsync();
            return users;
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
            catch(Exception ex)
            {
                _logger.LogError(ex, "Activate account error.");
                return false;
            }
        }

        public async Task<bool> ResetPassword(string id, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user is null)
            {
                _logger.LogError("Reset password error - user not found.");
                return false;
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                _logger.LogError("Reset password error.");
            }
            return result.Succeeded;
        }

        public async Task<EditAccountInput> GetEditUserInputById(string id)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(user is null)
            {
                _logger.LogError("Get edit user data error.");
                return new EditAccountInput();
            }
            return new EditAccountInput() { Id = user.Id, Name = user.Name, Surname = user.Surname, Email = user.Email, UserName = user.UserName };
        }

        public async Task<bool> UpdateUserInput(EditAccountInput input)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == input.Id);
            try
            {
                user.Name = input.Name;
                user.Surname = input.Surname;
                user.Email = input.Email;
                user.UserName = input.UserName;
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Update user data error.");
                return false;
            }
        }
    }
}
