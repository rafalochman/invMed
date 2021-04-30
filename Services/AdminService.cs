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

        public AdminService(ApplicationDbContext db)
        {
            _db = db;
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
                        UserName = u.Surname,
                        Email = u.Email,
                        Role = r.Name
                    })
                .ToListAsync();
        }

    }
}
