namespace DataLoader.Pages
{
    using System.Diagnostics.Metrics;
    using System.Threading.Tasks;
    using Dataloader.Api.DTO;
    using DataLoader.Services;
    using DataLoader.Services.InterFaces;
    using Microsoft.AspNetCore.Components;
    using Newtonsoft.Json.Linq;
    using Radzen;
    using Radzen.Blazor.Rendering;
    using Radzen.Blazor;

    public class UsermanagerBlazor: ComponentBase
    {
        [Inject]
        public IUserManagerService userManagerService { get; set; }
        [Inject]
        public NotificationService NotificationService { get; set; }

       public string username { get; set; }
       public string password { get; set; }
       public string role { get; set; }

        public List<UserDTO> userlist;


        protected async override Task OnInitializedAsync()
        {
            userlist = await userManagerService.getuserList();
        }


        public async Task CreateUser()
        {
            try { 
            await userManagerService.CreateUser(username, password, role);
                NotificationService.Notify(new NotificationMessage
                { Severity = NotificationSeverity.Success, Summary = "User created", Detail = $"User created with {username} username", Duration = 6000 });
            }
            catch
            {
                NotificationService.Notify(new NotificationMessage
                { Severity = NotificationSeverity.Warning, Summary = "User Not created", Detail = $"user {username} alredy exists", Duration = 6000 });


            }
        }

        public async Task RefreshList()
        {
            userlist = await userManagerService.getuserList();
            StateHasChanged();
        }

        public async Task Delete(string username)
        {
            try { 
            await userManagerService.DeleteUser(username);
           await RefreshList();
                NotificationService.Notify(new NotificationMessage
                { Severity = NotificationSeverity.Success, Summary = "User Deleted", Detail = $"User Deleted with {username} username", Duration = 6000 });
            }
            catch
            {
                NotificationService.Notify(new NotificationMessage
                { Severity = NotificationSeverity.Success, Summary = "User not deleted", Detail = $"problem during user deletion", Duration = 6000 });

            }
        }

        public bool showModal = false;
        public string selectedUsername;

        public void OpenModal(string username)
        {
            selectedUsername = username;
            showModal = true;
        }

        public async Task ChangePassword (string username, string password)
        {

            await userManagerService.ChangePassword(username, password);

        }

        public async Task ChangePasswordDialog(string username)
        {
         
        }




    }
}
