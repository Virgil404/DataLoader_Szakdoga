using Microsoft.AspNetCore.Identity;

namespace DataloaderApi.Data
{
    public class TaskData
    {
        public long Id { get; set; }

        public string TaskName { get; set; }

        public string TaskDescription { get; set; }

        public string sourceLocation { get; set; }

        public string DestinationTable { get; set; }

        public bool isActive { get; set; }

        public ICollection<ApplicationUser> AssignedUsers { get; set; }



    }
}
