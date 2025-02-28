using Dataloader.Api.DTO;
using DataloaderApi.Data;
using Microsoft.AspNetCore.Identity;

namespace DataloaderApi.Dao
{
    public interface IAuthHandlingDao
    {
        public  Task<bool> CreateUser (string username, string password, string role);

        public Task<bool> ChangePassword(string username,string password);

        public Task<bool> DeleteUser (string username);

        //public Task<bool> userExitsWithUserName(string username);
        // private Task<IdentityUser>? GetUserByUserName(string username);

        public Task<List<UserDTO>> GetUsers();
    }
}
