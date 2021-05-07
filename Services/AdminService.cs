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
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AspNetUser> _userManager;

        public AdminService(ApplicationDbContext db, UserManager<AspNetUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<List<UserView>> GetUsers()
        {
            return await (from u in _db.Users
                    join ur in _db.UserRoles on u.Id equals ur.UserId
                    join r in _db.Roles on ur.RoleId equals r.Id
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
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUser(AspNetUser user)
        {
            _db.Users.Remove(user);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddUser(AspNetUser user, string password)
        {
            await _userManager.CreateAsync(user, password);
            return true;
        }

        public async Task<bool> AddToRole(AspNetUser user, string role)
        {
            await _userManager.AddToRoleAsync(user, role);
            return true;
        }

        public async Task<bool> RemoveFromRole(AspNetUser user, string role)
        {
            await _userManager.RemoveFromRoleAsync(user, role);
            return true;
        }
    }
}
