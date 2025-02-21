using DataloaderApi.Data;

namespace DataloaderApi.Dao
{
    public interface IAuthHandlingDao
    {
        public  Task<bool> CreateUser (string username, string password, string role);

        public Task<bool> ChangePassword(string username,string password);

        public Task<bool> DeleteUser (string username);

        public Task<bool> userExitsWithUserName(string username);
        public Task<User>? GetUserByUserName(string username);

        public Task<bool> insertRefreshToken(RefreshToken refreshToken, string username);

        public Task<bool> disableuserTokenByuserName(string userName);

        public  Task<bool> disableUserTokenByToken(string token);

        public Task<bool> isTokenValid(string token);

        public Task<User> findUserByToken(string token);
    }
}
