namespace DataLoader.Services.InterFaces
{
    public interface IAccessTokenService
    {

        public  Task SetToken(string token);

        public Task<string> GetToken();

        public Task RemoveToken(string token);

    }
}
