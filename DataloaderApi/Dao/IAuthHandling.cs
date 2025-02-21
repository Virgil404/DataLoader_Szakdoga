namespace DataloaderApi.Dao
{
    public interface IAuthHandling
    {
        public  Task<bool> CreateUser (string username, string password, string role);

        public Task<bool> ChangePassword(string username,string password);

        public Task<bool> DeleteUser (string username);


    }
}
