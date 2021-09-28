using invMed.Data;
using System.Threading.Tasks;

namespace invMed.Services
{
    public interface IAccountService
    {
        Task<bool> ChangeUserPassword(string userName, string currentPassword, string newPassword);
        Task<UserView> GetUserByName(string userName);
    }
}