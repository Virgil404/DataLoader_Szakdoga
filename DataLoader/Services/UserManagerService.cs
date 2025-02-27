using System.Net.Http.Json;
using System.Text;
using Dataloader.Api.DTO;
using DataLoader.Services.InterFaces;

namespace DataLoader.Services
{
    public class UserManagerService :IUserManagerService
    {
        private readonly HttpClient httpClient;

        public UserManagerService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task ChangePassword(string username ,string password)
        {

            var url = $"/api/Auth/ChangePassword?username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}";
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            using var response = await httpClient.PutAsync(url, content);
            Console.WriteLine("Response: " + response);
            if (!response.IsSuccessStatusCode)
            {
                string error = response.ReasonPhrase;
                throw new Exception(error);
            }


        }

        public async Task CreateUser(string username, string password, string role)
        {
            var url = $"/api/Auth/createUser?username={Uri.EscapeDataString(username)}&password={Uri.EscapeDataString(password)}&role={Uri.EscapeDataString(role)}";
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            using var response = await httpClient.PostAsync(url, content);

            Console.WriteLine("Response: " + response);
            if (!response.IsSuccessStatusCode)
            {
                string error = response.ReasonPhrase;
                throw new Exception(error);
            }

        }

        public async Task DeleteUser(string username)
        {

            try
            {
                using var response = await httpClient.DeleteAsync($"/api/Auth/deleteUser?username={username}");

                if (!response.IsSuccessStatusCode)
                {

                    string error = response.ReasonPhrase;
                    throw new Exception(error);


                }
            }
            catch (Exception)
            {

                throw;
            }

          



        }

        public async Task<List<UserDTO>> getuserList()
        {

            try
            {
                var userlist = await httpClient.GetFromJsonAsync<List<UserDTO>>("/api/Auth/getuserlist");

                return userlist;

            }
            catch (Exception)
            {

                throw;

            }

        }
    }
}
