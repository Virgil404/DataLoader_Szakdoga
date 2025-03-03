using Dataloader.Api.DTO;
using Microsoft.AspNetCore.Identity;

namespace DataloaderApi.Data
{
    public class ApplicationUser:IdentityUser
    {


        public ICollection<TaskData> Tasks { get; set; }

    }
}
