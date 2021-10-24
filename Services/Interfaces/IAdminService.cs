using invMed.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace invMed.Services
{
    public interface IAdminService
    {
        Task<bool> ActivateAccount(string userId);
        Task<bool> AddToRole(CreateAccountInput input);
        Task<bool> AddToRole(string id, string role);
        Task<bool> AddUser(CreateAccountInput input);
        Task<bool> DeactivateAccount(string id);
        Task<List<UserView>> GetDeactivatedUsers();
        Task<EditAccountInput> GetEditUserInputById(string id);
        Task<UserView> GetUserById(string id);
        Task<List<UserView>> GetUsers();
        Task<bool> RemoveFromRole(string id, string role);
        Task<bool> ResetPassword(string id, string newPassword);
        Task<bool> UpdateUserInput(EditAccountInput input);
    }
}