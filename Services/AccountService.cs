using System;
using System.Threading.Tasks;
using invMed.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace invMed.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<AspNetUser> _userManager;
        private readonly SignInManager<AspNetUser> _signInManager;
        private readonly ILogger<IAccountService> _logger;

        public AccountService(ApplicationDbContext db, UserManager<AspNetUser> userManager, SignInManager<AspNetUser> signInManager, ILogger<IAccountService> logger)
        {
            _db = db;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<UserView> GetUserByName(string userName)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            try
            {
                var role = await _userManager.GetRolesAsync(user);
                return new UserView(user, role[0]);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Get user by name error.");
                return new UserView();
            }
        }

        public async Task<bool> ChangeUserPassword(string userName, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if(user is null)
            {
                _logger.LogError("Change user password error - user not found.");
                return false;
            }
            var passwordValidation = await _signInManager.UserManager.CheckPasswordAsync(user, currentPassword);
            if (passwordValidation)
            {
                try
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                    return result.Succeeded;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Change user password error.");
                    return false;
                }
            }
            else
            {
                _logger.LogError("Change user password error - password not valid.");
                return false;
            }
        }
    }
}
