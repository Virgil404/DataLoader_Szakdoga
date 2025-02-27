
using System.Reflection.Metadata.Ecma335;
using BCrypt.Net;
using DataloaderApi.Data;
using Microsoft.EntityFrameworkCore;
using Dataloader.Api.DTO;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace DataloaderApi.Dao
{
    public class AuthHandlingDao : IAuthHandlingDao


    {

        private readonly Applicationcontext _context;
        private readonly UserManager<IdentityUser> userManager;
        private readonly IdentityContext _identityContext;
        public AuthHandlingDao(Applicationcontext context, UserManager<IdentityUser> userManager, IdentityContext identityContext)
        {
            _context = context;
            this.userManager = userManager;
            _identityContext = identityContext;
        }

        public async Task<bool> CreateUser(string username, string password , string Role)
        {
            throw new System.NotImplementedException();
        }


        public async Task<bool> ChangePassword(string username, string Password)
        {

            var user = await userManager.FindByEmailAsync(username);
            if (user == null) return false;
            await userManager.ResetPasswordAsync(user, userManager.GeneratePasswordResetTokenAsync(user).Result, Password);
            return true;
        }

        public async Task<bool> DeleteUser(string username)
        {

            var userdelete = await userManager.FindByEmailAsync(username);
            if (await userExitsWithUserName(username))
            {
                await userManager.DeleteAsync(userdelete);
                return true;
            }
            return false;

        }





        public async Task<bool> userExitsWithUserName(string username)
        {
          var userexist = await userManager.FindByEmailAsync(username);
            if (userexist == null) return false;
            return true;
        }


        public async Task<List<UserDTO>> GetUsers()
        {
            var userlist = userManager.Users.ToList();
            var userdtolist = new List<UserDTO>();
            foreach (var user in userlist)
            {
               userdtolist.Add(new UserDTO { username = user.Email, Role =  userManager.GetRolesAsync(user).ToString() });
            }

            return userdtolist;
        }



    }
}
