using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web;
using Domain.Data.Identity;
using Domain.Data.Identity.Aspnet;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;
using Repositories.Interfaces;
using Services.Interfaces;

namespace Services
{
    public class UsersService : IUsersService
    {
        private readonly IRecoverPasswordRepository _recoverPasswordRepository;
        private readonly IEmailService _emailServices;

        public UsersService(IEmailService emailServices
                               , IRecoverPasswordRepository recoverPasswordRepository
            )
        {
            _emailServices = emailServices;
            _recoverPasswordRepository = recoverPasswordRepository;
        }

        private ApplicationUserManager UserManager => Managers.Users;

        public async Task<ApplicationUser> GetUser(string userId)
        {
            var result = await UserManager.FindByIdAsync(userId);
            return result;
        }

        public async Task<string> IsValid(PreRegisteredUser user)
        {
            await Task.Run(() =>
            {
                if (string.IsNullOrEmpty(user.UserName))
                    return "User name is mandatory";

                if (string.IsNullOrEmpty(user.EmailAddress))
                    return "Email address is mandatory";

                if (HttpContext.Current == null)
                    return null;

                var userExists = UserManager.FindByNameAsync(user.UserName).Result;

                return userExists != null ? "The user name selected is already taken" : null;
            });
            return string.Empty;
        }

        public void DeleteUser(ApplicationUser user)
        {
            UserManager.Delete(user);
        }

        public async Task<UsernameValidityState> UsernameIsValidAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
                return UsernameValidityState.Empty;

            var userExists = await UserManager.FindByNameAsync(username);
            return userExists != null ? UsernameValidityState.Taken : UsernameValidityState.Ok;
        }

        public string TryToSendEmailRecovery(string username)
        {
            var userExists = UserManager.FindByName(username);

            if (userExists == null)
                return String.Empty;

            var valid = ConfigurationManager.AppSettings["ValidPreregistrationDays"] ?? "5";
            var validUntil = DateTime.UtcNow.AddDays(Convert.ToInt32(valid));
            var token = Guid.NewGuid().ToString().Replace("-", "");
            var recoverPassword = new RecoverPassword
            {
                UserName = username,
                ValidUntil = validUntil,
                Token = token,
                Activated = false
            };
            _recoverPasswordRepository.AddAsync(recoverPassword).Wait();

            var templateDictionary = new Dictionary<string, string>
            {
                {
                    "TOKEN",
                    $"{ConfigurationManager.AppSettings["WebRoute"]}/login/recoverpassword?token={token}"
                },
                {
                    "NAME", 
                    username
                }
            };
            const string lang = "en-US";

            Task.Run(()=> _emailServices.SendEmail(userExists.Email, "Recover Password", "RecoverPassword", templateDictionary, lang));
            return token;
        }

        public RecoverPassword GetRecoverPasswordByToken(string token, out int status)
        {
            var recoverPassword = _recoverPasswordRepository.Collection.Find(x => x.Token == token).FirstOrDefaultAsync().Result;

                if (recoverPassword == null)
                {
                    status = 404; // not found
                    return null;
                }
                if (recoverPassword.ValidUntil <= DateTime.UtcNow)
                {
                    status = 400; // bad request
                    return null;
                }
                status = 200;   // found ok
                return recoverPassword;
        }

        public int CanChangePassword(string token)
        {
            var recoverPassword = _recoverPasswordRepository.Collection.Find(x => x.Token == token).FirstOrDefaultAsync().Result;

                if (recoverPassword == null)
                {
                    return 404; // not found
                }
                if (recoverPassword.ValidUntil <= DateTime.UtcNow)
                {
                    return 400; // bad request
                }
                return 200;   // found ok
        }

        public async Task<string> GetUserId(string userName)
        {
            var result = await UserManager.FindByNameAsync(userName);

            return result?.Id;
        }
    }
}

