
using System.ComponentModel.DataAnnotations;


namespace Dataloader.Api.DTO
{
    public class RegisterDTO
    {

        [Required, EmailAddress]
        public string email { get; set; }

        [Required(ErrorMessage ="Username is required")]

        public string username { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public string password { get; set; }


        public string role { get; set; } 
    }
}
