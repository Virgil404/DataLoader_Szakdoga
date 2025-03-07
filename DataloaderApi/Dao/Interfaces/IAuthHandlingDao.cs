using Dataloader.Api.DTO;
using DataloaderApi.Data;
using Microsoft.AspNetCore.Identity;

namespace DataloaderApi.Dao.Interfaces
{
    public interface IAuthHandlingDao
    {
        public  Task<bool> CreateUser (RegisterDTO registerDTO);

        public Task<bool> ChangePassword(string username,string password);

        public Task<bool> DeleteUser (string username);

        //public Task<bool> userExitsWithUserName(string username);
        // private Task<IdentityUser>? GetUserByUserName(string username);

        public Task<List<UserDTO>> GetUsers();

        public Task<bool> changeRole(string username,string role);

        public Task assignUserToTask(string jobname, string username);
    }
}
