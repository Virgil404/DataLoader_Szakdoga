
namespace Dataloader.Api.DTO
{
   public class DetailedTaskDTO
    {

        public string CratedAt { get; set; }
        public string LastExecution { get; set; }

        public string NextExecution { get; set; }

        public string JobID { get; set; }

        public string Cron { get; set; }

        public string TaskDescription { get; set; }

        public string sourceLocation { get; set; }

        public string DestinationTable { get; set; }
    }
}
