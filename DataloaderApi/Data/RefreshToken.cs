using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataloaderApi.Data
{
    public class RefreshToken
    {

        [Key] [Required]
        public string Token { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now; 

        public DateTime  Expires {  get; set; }

        public bool enabled { get; set; }

        // Foreign Key
        [ForeignKey("User")]
        [Required]
        public string Username { get; set; }

        // Navigation Property
        public User User { get; set; }
    }
}
