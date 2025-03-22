namespace DataLoader.Pages
{
    using System.Threading.Tasks;
    using Dataloader.Api.DTO;
    using DataLoader.Services.InterFaces;
    using Microsoft.AspNetCore.Components;
    using Radzen;
    using Microsoft.IdentityModel.Tokens;
    using System.Text.RegularExpressions;

    public class UsermanagerBlazor: ComponentBase
    {
        [Inject]
        public IUserManagerService userManagerService { get; set; }
        [Inject]
        public NotificationService NotificationService { get; set; }

       public string username { get; set; }
       public string password { get; set; }
        public string email { get; set; }
        public string role { get; set; }

        public List<UserDTO> userlist;


        protected async override Task OnInitializedAsync()
        {
            userlist = await userManagerService.getuserList();
        }


        public async Task CreateUser()
        {
            try {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(role) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(email))
                {
                    NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Invalid Input", Detail = "One or more required fields are empty", Duration = 6000 });
                    return;
                }

                if (password.Length<5)
                {
                    NotificationService.Notify(new NotificationMessage
                    { Severity = NotificationSeverity.Warning, Summary = "User Not created", Detail = "Password must be at least 5 character", Duration = 6000 });
                    return;
                }

                if (!isEmailValid(email))
                {
                    NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "User Not Created", Detail = "Email is not valid", Duration = 6000 });
                    return;
                }
            
                var user = new RegisterDTO
                {

                    username = username,
                    email = email,
                    role = role,
                    password = password 
                };

            await userManagerService.CreateUser(user);
                NotificationService.Notify(new NotificationMessage
                { Severity = NotificationSeverity.Success, Summary = "User created", Detail = $"User created with {username} username", Duration = 6000 });
            }
            catch(Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                { Severity = NotificationSeverity.Warning, Summary = "User Not created", Detail = $"Problem: {ex.Message}", Duration = 6000 });


            }
        }

        private bool isEmailValid(string email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return false; 
            }

            try
            {
                var emailPattern= @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, emailPattern, RegexOptions.IgnoreCase);
            }
            catch 
            {

                return false;
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

            if (password.IsNullOrEmpty())
            {
                NotificationService.Notify(new NotificationMessage 
                { Severity = NotificationSeverity.Warning, Summary = "cannot change password", Detail = "password field is empty", Duration = 4000 });
            }
            else { 

                await userManagerService.ChangePassword(username, password);

            NotificationService.Notify(new NotificationMessage
            { Severity = NotificationSeverity.Success, Summary = "Password Changed successfully", Duration = 4000 });
            }
        }





    }
}
