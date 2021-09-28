using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using invMed.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace invMed.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly SignInManager<AspNetUser> _signInManager;

        public AccountService(ApplicationDbContext db, UserManager<AspNetUser> userManager, SignInManager<AspNetUser> signInManager)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<UserView> GetUserByName(string userName)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            var role = await _userManager.GetRolesAsync(user);
            return new UserView() { Id = user.Id, Name = user.Name, Surname = user.Surname, UserName = user.UserName, Email = user.Email, Role = role[0] };
        }

        public async Task<bool> ChangeUserPassword(string userName, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var passwordValidation = await _signInManager.UserManager.CheckPasswordAsync(user, currentPassword);
            if (passwordValidation)
            {
                try
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                    return result.Succeeded;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
