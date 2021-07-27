using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace invMed.Services
{
    public class AdminService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _dbFactory;
        private readonly UserManager<AspNetUser> _userManager;

        public AdminService(IDbContextFactory<ApplicationDbContext> dbFactory, UserManager<AspNetUser> userManager)
        {
            _dbFactory = dbFactory;
            _userManager = userManager;
        }

        public async Task<List<UserView>> GetUsers()
        {
            using var db = _dbFactory.CreateDbContext();
            return await (from u in db.Users
                          where u.IsActive == true
                          join ur in db.UserRoles on u.Id equals ur.UserId
                          join r in db.Roles on ur.RoleId equals r.Id
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
            using var db = _dbFactory.CreateDbContext();
            return await db.Users.FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<bool> UpdateUser(AspNetUser user)
        {
            using var db = _dbFactory.CreateDbContext();
            try
            {
                db.Users.Update(user);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeactivateAccount(AspNetUser user)
        {
            using var db = _dbFactory.CreateDbContext();
            user.IsActive = false;
            try
            {
                db.Users.Update(user);
                await db.SaveChangesAsync();
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
            using var db = _dbFactory.CreateDbContext();
            return await (from u in db.Users
                          where u.IsActive == false
                          join ur in db.UserRoles on u.Id equals ur.UserId
                          join r in db.Roles on ur.RoleId equals r.Id
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
            using var db = _dbFactory.CreateDbContext();
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                user.IsActive = true;
                db.Users.Update(user);
                await db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
