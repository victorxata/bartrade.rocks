using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Data.Identity;

namespace Services.Interfaces
{
    public interface IUsersService
    {
        Task<ApplicationUser> GetUser(string userId);

       Task<UsernameValidityState> UsernameIsValidAsync(string username);

      string TryToSendEmailRecovery(string username);

        RecoverPassword GetRecoverPasswordByToken(string token, out int status);

        int CanChangePassword(string token);

        Task<string> GetUserId(string userName);

        void DeleteUser(ApplicationUser user);
    }
}
