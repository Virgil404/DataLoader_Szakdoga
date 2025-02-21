using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace DataloaderApi.Data
{
    public class User
    {

        /*
        public User(string UserID, string Password, string Role) 
        {
        
            this.UserID = UserID;
            this.Password = Password;
            this.Role = Role;
        
        }
        */

        [Key]
        public  string UserID { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Role { get; set; }
    }
}
