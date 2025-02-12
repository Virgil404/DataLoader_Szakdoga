using DataloaderApi.DataRead;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dataloader.Api.DTO;
namespace DataloaderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        private readonly DataProcess _dataProcess;

        public HangfireController(DataProcess dataProcess)
        {

            _dataProcess = dataProcess;
        }

        private void CreateRecurringJob(string cron, string jobname, string filepath, string delimiter, bool hasheader, string tableName)
        {

            Console.WriteLine(jobname + " started");
            RecurringJob.AddOrUpdate(jobname, () => _dataProcess.readAndInsert(filepath, delimiter, hasheader, tableName), cron);


        }

        private void CreateRecurringJobWithoutDelete(string cron, string jobname, string filepath, string delimiter, bool hasheader, string tableName)
        {

            Console.WriteLine(jobname + " started");
            RecurringJob.AddOrUpdate(jobname, () => _dataProcess.readAndInsertWithoutDelete(filepath, delimiter, hasheader, tableName), cron);

        }

        [HttpPost("CreateTask")]
        public async Task<ActionResult> createTask(string cron, string jobname, string filePath, string delimiter, bool hasheader, string tableName)
        {
            try {
                CreateRecurringJob(cron, jobname, filePath, delimiter, hasheader, tableName);
                return Ok();
            }
            catch {

                return BadRequest();
            }

        }

        [HttpPost( "CreatenonDeleteTask")]
        public async Task<ActionResult> createnoDeleteTask(string cron, string jobname, string filePath, string delimiter, bool hasheader, string tableName)
        {

            try
            {
                CreateRecurringJobWithoutDelete(cron, jobname, filePath, delimiter, hasheader, tableName);
                return Ok();
            }
            catch
            {

                return BadRequest();
            }

        }

         
        [HttpGet("getRecurrningJobs")]
        public async Task<ActionResult<List<RecurringJobDto>>> getrecurringjobs()
        {

           
            try {
            var joblist = new List<RecurringJobDto>();
             var jobdtolist = new List<TaskDTO>();
            joblist =  Hangfire.JobStorage.Current.GetConnection().GetRecurringJobs().ToList();


                foreach (var job in joblist) {


                   
                    jobdtolist.Add(new TaskDTO(job.CreatedAt.ToString(),job.LastExecution.ToString(),job.NextExecution.ToString(),job.Id.ToString(), job.Cron.ToString()));
                
                }

            return Ok(jobdtolist);

            }
            catch { return BadRequest(); }

        }


        [HttpDelete("DeleteTask")]
        public async Task<ActionResult> deletetask(string taskid)
        {
            try
            {
                RecurringJob.RemoveIfExists(taskid);
                Console.WriteLine("Task Deleted");
                return Ok();
            }
            catch {
                return BadRequest();
            }
        }

        [HttpPost("triggerjob")]

        public async Task<ActionResult> triggerJob(string taskid)
        {
            try
            {

                RecurringJob.TriggerJob(taskid);
                return Ok();

            }
            catch
            {

                return BadRequest();

            }
        }
    }
}
