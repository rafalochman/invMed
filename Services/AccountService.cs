using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace invMed.Services
{
    public class AccountService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AspNetUser> _userManager;

        public AccountService(ApplicationDbContext db, UserManager<AspNetUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<UserView> GetUserByName(string userName)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            var role = await _userManager.GetRolesAsync(user);
            return new UserView() { Id = user.Id, Name = user.Name, Surname = user.Surname, UserName = user.UserName, Email = user.Email, Role = role[0] };
        }
    }
}
