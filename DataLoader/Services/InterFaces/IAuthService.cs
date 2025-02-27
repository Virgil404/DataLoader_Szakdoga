namespace DataLoader.Services.InterFaces
{
    public interface IAuthService
    {

        public Task<bool> Login (string username, string password);



    }
}
