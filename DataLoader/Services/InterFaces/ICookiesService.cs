namespace DataLoader.Services.InterFaces
{
    public interface ICookiesService
    {

        public Task<string> GetCookie(string key);

        public Task RemoveCookie (string key);

        public Task SetCookie (string key, string value, int days);


    }
}
