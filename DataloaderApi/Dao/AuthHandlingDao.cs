﻿using Dataloader.Api.DTO;
using DataloaderApi.Dao.Interfaces;
using DataloaderApi.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataloaderApi.Dao
{
    public class AuthHandlingDao : IAuthHandlingDao


    {

        private readonly Applicationcontext _context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IdentityContext _identityContext;
      //  private readonly RoleManager<IdentityUser> _roleManager;
        public AuthHandlingDao(Applicationcontext context, UserManager<ApplicationUser> userManager, IdentityContext identityContext )
        {
            _context = context;
            this.userManager = userManager;
            _identityContext = identityContext;
           // _roleManager = roleManager;
        }

        public async Task<bool> CreateUser(RegisterDTO registerDTO)
         {

            try { 
          
                var userexist = await userExitsWithUserName(registerDTO.username);
                if (userexist)
                {
                    throw new Exception("User Alredy Created");
                }


            var result = await userManager.CreateAsync(new ApplicationUser { Email = registerDTO.email, UserName = registerDTO.username }, registerDTO.password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(await userManager.FindByNameAsync(registerDTO.username), registerDTO.role);
                return true;
            }
                var errors = result.Errors.ToList();
                var errorsastring = String.Join(",", errors);
             throw new Exception(errorsastring);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task assignUserToTask(string jobname, string username)
        {
            var user = await GetUserByUserName(username);
            var taskdata = await _identityContext.TaskData
                                    .Include(t => t.AssignedUsers) 
                                    .FirstOrDefaultAsync(x => x.TaskName == jobname);

            if (taskdata != null && user != null)
            {
                if (!taskdata.AssignedUsers.Contains(user))
                {
                    _identityContext.Attach(user);
                    taskdata.AssignedUsers.Add(user);
                    await _identityContext.SaveChangesAsync();
                }
                else throw new Exception("User alredy Assigned");
            }
            else throw new Exception("User or Task not exist");
        }

        public async Task<bool> ChangePassword(string username, string Password)
        {
           
            var user = await GetUserByUserName(username);
            if (user == null) return false;
            await userManager.ResetPasswordAsync(user, userManager.GeneratePasswordResetTokenAsync(user).Result, Password);
            return true;
        }

        public async Task<bool> DeleteUser(string username)
        {

            var userdelete = await GetUserByUserName(username);
            if (userdelete == null) return false;
            if (await userExitsWithUserName(username))
            {
                await userManager.DeleteAsync(userdelete);
                return true;
            }
            return false;

        }






        private async Task<bool> userExitsWithUserName(string username)
        {
          var userexist = await GetUserByUserName(username);
            if (userexist == null) return false;
            return true;
        }

      
        public async Task<List<UserDTO>> GetUsers()
        {
            var userlist = userManager.Users.ToList();
            var userdtolist = new List<UserDTO>();
            foreach (var user in userlist)
            {
                var currentuserrole =  await userManager.GetRolesAsync(user);
                
                userdtolist.Add(new UserDTO { username = user.UserName, email=user.Email ,Role = String.Join(",", currentuserrole) });
            }

            return userdtolist;
        }

        public async Task<ApplicationUser>? GetUserByUserName(string username)
        {
            try { 
            var user = await userManager.FindByNameAsync(username);
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public async Task<bool> changeRole(string username, string role)
        {
            try { 
            var user = await GetUserByUserName(username);

            var userrole = await userManager.GetRolesAsync(user);
               
           await  userManager.RemoveFromRoleAsync(user, userrole[0]);
            await userManager.AddToRoleAsync(user, role);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
