namespace Dataloader.Api.DTO
{
    public class TaskDTO
    {

        public TaskDTO(string CreatedAt, string LastExecution, string NextExecution, string JobID, string Cron)
        {

            CratedAt = CreatedAt;
            this.LastExecution = LastExecution;
            this.NextExecution = NextExecution;
            this.JobID = JobID;
            this.Cron = Cron;


        }

        public TaskDTO() { }


        public string CratedAt { get; set; }
        public string LastExecution { get; set; }

        public string NextExecution { get; set; }

        public string JobID { get; set; }

        public string Cron { get; set; }

    }
}
