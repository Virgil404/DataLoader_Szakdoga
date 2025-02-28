using Dataloader.Api.DTO;

namespace DataLoader.Services.InterFaces
{
    public interface IUserManagerService
    {

        public  Task CreateUser (RegisterDTO register);

        public Task DeleteUser ( string username);

        public Task ChangePassword(string username,string password);

        public Task<List<UserDTO>> getuserList();



    }
}
