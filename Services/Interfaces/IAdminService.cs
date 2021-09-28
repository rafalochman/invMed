using invMed.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace invMed.Services
{
    public interface IAdminService
    {
        Task<bool> ActivateAccount(string userId);
        Task<bool> AddToRole(AspNetUser user, string role);
        Task<bool> AddUser(AspNetUser user, string password);
        Task<bool> DeactivateAccount(AspNetUser user);
        Task<List<UserView>> GetDeactivatedUsers();
        Task<EditAccountInput> GetEditUserInputById(string id);
        Task<AspNetUser> GetUserById(string id);
        Task<List<UserView>> GetUsers();
        Task<bool> RemoveFromRole(AspNetUser user, string role);
        Task<bool> ResetPassword(AspNetUser user, string newPassword);
        Task<bool> UpdateUser(AspNetUser user);
        Task<bool> UpdateUserInput(EditAccountInput input);
    }
}