namespace DataloaderApi.Data.DTO
{
    public class job
    {

        public job(string CreatedAt, string LastExecution, string NextExecution, string JobID, string Cron) {
            
            this.CratedAt= CreatedAt;
            this.LastExecution= LastExecution;
            this.NextExecution= NextExecution;
            this.JobID= JobID;
            this.Cron= Cron;
        
        
        }


        public string CratedAt { get; set; }
        public string LastExecution { get; set; }

        public string NextExecution { get; set; }

        public string JobID { get; set; }

        public string Cron { get; set; }

    }
}
