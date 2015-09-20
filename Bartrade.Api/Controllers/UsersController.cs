using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using Api.Common.Attributes;
using Domain.Data.Identity;
using Domain.Data.Identity.Aspnet;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;

namespace Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    [Validation]
    [RoutePrefix("api")]
    public class UsersController : ApiController
    {
        private readonly ApplicationUserManager _userManager = Managers.Users;

        /// <summary>
        /// Returns the complete list of users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        [Route("Users")]
        public IHttpActionResult GetProfiles()
        {
            var users = _userManager.Users.ToList();

            return Ok(users);
        }

        /// <summary>
        /// Returns the complete list of users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Route("Users/{userId}")]
        public async Task<IHttpActionResult> GetUserProfile(string userId)
        {
            var currentUser = ActionContext.RequestContext.Principal.Identity;
            if ((currentUser.GetUserId() != userId) && (!_userManager.IsInRole(currentUser.GetUserId(), "Admin")))
                return Unauthorized();

            var user = await _userManager.FindByIdAsync(userId);

           return Ok(user);
        }

        /// <summary>
        /// Returns a user by his name
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles="Admin")]
        [Route("Users/ByName/{userName}")]
        public async Task<IHttpActionResult> GetUserProfileByName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            return Ok(user);
        }

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("Users")]
        public async Task<IHttpActionResult> AddUser([FromBody] RegisterModel model)
        {
            var user = new ApplicationUser();
            user.ApplyChanges(model);

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                return Ok("Registered");
            }

            return new BadRequestResult(new HttpRequestMessage(HttpMethod.Post, result.Errors.ToString()));
        }

        /// <summary>
        /// Adds a new user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        [Route("Users/{userId}")]
        public async Task<IHttpActionResult> UpdateUser(string userId, [FromBody] ApplicationUser model)
        {
            var currentUser = ActionContext.RequestContext.Principal.Identity;

            if ((currentUser.GetUserId() != userId) && (!_userManager.IsInRole(currentUser.GetUserId(), "Admin")))
                return Unauthorized();

            var user = model;

            var oldUser = await _userManager.FindByIdAsync(userId);

            model.PasswordHash = oldUser.PasswordHash;

            await Managers.UsersCollection.ReplaceOneAsync(x => x.Id == model.Id, user,
                    new UpdateOptions {IsUpsert = true});
            
            var result = await _userManager.FindByIdAsync(userId);
            return Ok(result);
        }

        /// <summary>
        /// Deletes an existing user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("Users/{userId}")]
        public async Task<IHttpActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();
            var result = await _userManager.DeleteAsync(user);
            return Ok(result.Succeeded);
        }

        /// <summary>
        /// Sets a user as SuperAdmin
        /// </summary>
        /// <param name="userId">The Id of the user</param>
        /// <returns>The user object updated</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Users/{userId}/ToSuper")]
        public async Task<IHttpActionResult> SetToSuper(string userId)
        {
            if (await _userManager.FindByIdAsync(userId) != null)
            {
                await _userManager.AddUserToRolesAsync(userId, new List<string> { "Admin" });
                var result = await _userManager.FindByIdAsync(userId);
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        /// Removes a user from the super admin group
        /// </summary>
        /// <param name="userId">The Id of the user</param>
        /// <returns>The user object updated</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Users/{userId}/ToNormal")]
        public async Task<IHttpActionResult> SetToNormal(string userId)
        {
            if (await _userManager.FindByIdAsync(userId) != null)
            {
                await _userManager.RemoveUserFromRolesAsync(userId, new List<string> { "Admin" });
                var result = await _userManager.FindByIdAsync(userId);
                return Ok(result);
            }
            return NotFound();
        }

        /// <summary>
        /// Changes a user password
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("Users/{UserId}/ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(string userId, [FromBody]ChangePasswordModel model)
        {
            var currentUser = ActionContext.RequestContext.Principal.Identity;
            
            if ((currentUser.GetUserId() != userId) && (!_userManager.IsInRole(currentUser.GetUserId(), "Admin")))
                return Unauthorized();

            var result = await _userManager.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);
            var errorResult = GetErrorResult(result);
            if (errorResult != null)
            {
                return errorResult;
            }
            
            return Ok("Password changed");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [Route("Users/{userId}/SetPassword")]
        public async Task<IHttpActionResult> SetPassword(string userId, [FromBody] SetPasswordModel model)
        {
            var currentUser = ActionContext.RequestContext.Principal.Identity;
            
            if ((currentUser.GetUserId() != userId) && (!_userManager.IsInRole(currentUser.GetUserId(), "Admin")))
                return Unauthorized();

            await _userManager.RemovePasswordAsync(userId);
            var result = await _userManager.AddPasswordAsync(userId, model.Password);

            var errorResult = GetErrorResult(result);
            if (errorResult != null)
            {
               return errorResult;
            }

            return Ok("Password set");
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (result.Succeeded) return null;

            if (result.Errors != null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Error", error);
                }
            }

            return BadRequest(ModelState);
        }
    }
}
