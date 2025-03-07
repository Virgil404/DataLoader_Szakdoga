using DataloaderApi.DataRead;
using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Dataloader.Api.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DataloaderApi.Data;
using DataloaderApi.Dao;
using DataloaderApi.Dao.Interfaces;
namespace DataloaderApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireController : ControllerBase
    {
        private readonly DataProcess _dataProcess;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthHandlingDao _authHandlingDao;
        public HangfireController(DataProcess dataProcess,UserManager<ApplicationUser> userManager, IAuthHandlingDao authHandlingDao)
        {

            _dataProcess = dataProcess;
            _userManager = userManager;
            _authHandlingDao = authHandlingDao;
        }


        private async void CreateRecurringJob(string cron, string jobname, string filepath, string delimiter, bool hasheader, string tableName)
        {
            try
            {
                Console.WriteLine(jobname + " started");
                RecurringJob.AddOrUpdate(jobname, () => _dataProcess.readAndInsert(filepath, delimiter, hasheader, tableName), cron);
            }
            catch (Exception ex) {
                throw ex;
                    
                 }

        }

        private void CreateRecurringJobWithoutDelete(string cron, string jobname, string filepath, string delimiter, bool hasheader, string tableName)
        {

            Console.WriteLine(jobname + " started");
            RecurringJob.AddOrUpdate(jobname, () => _dataProcess.readAndInsertWithoutDelete(filepath, delimiter, hasheader, tableName), cron);

        }

        [Authorize]
        [HttpPost("CreateTask")]
        public async Task<ActionResult> createTask(string cron, string jobname, string filePath, string delimiter, bool hasheader, string tableName, string description)
        {
            try {
                var recurringJobs = Hangfire.JobStorage.Current.GetConnection().GetRecurringJobs();
                var jobexists = recurringJobs.Any(x => x.Id == jobname);
                var currentuser = await _userManager.GetUserAsync(User);
                if (!jobexists)
                {
                    await _dataProcess.InsertToTaskData(jobname, filePath, tableName,currentuser, description);
                }
                else
                {
                    await _dataProcess.UpdateTaskData(jobname, filePath, tableName, description);
                }
                CreateRecurringJob(cron, jobname, filePath, delimiter, hasheader, tableName);
                return Ok("Task Created");
            }
            catch (Exception ex) {

                return BadRequest("Error While Creating Task "+ ex.ToString());
            }

        }

        [HttpPost( "CreatenonDeleteTask")]
        public async Task<ActionResult> createnoDeleteTask(string cron, string jobname, string filePath, string delimiter, bool hasheader, string tableName)
        {

            try
            {
                CreateRecurringJobWithoutDelete(cron, jobname, filePath, delimiter, hasheader, tableName);
                return Ok("Task Created");
            }
            catch
            {

                return BadRequest("Error While Creating Task");
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
            catch { return BadRequest("Error While Returning Task"); }

        }


        [HttpGet("getJobsAssignedToUser")]
        [Authorize]
        public async Task <ActionResult<List<DetailedTaskDTO>>> getJobsAssignedToUser()
        {
            try
            {
                var joblist = new List<RecurringJobDto>();
                var jobdtolist = new List<DetailedTaskDTO>();
                joblist = Hangfire.JobStorage.Current.GetConnection().GetRecurringJobs().ToList();

                var currentuser = await _userManager.GetUserAsync(User);
                var tasklist = _dataProcess.getTaskData(currentuser);
                var filteredtasklist = joblist.Where(x => tasklist.Any(y => y.TaskName == x.Id)).ToList();

                foreach (var task in filteredtasklist)
                {
                    jobdtolist.Add(new DetailedTaskDTO
                    {
                        CratedAt = task.CreatedAt.ToString(),
                        LastExecution = task.LastExecution.ToString(),
                        NextExecution = task.NextExecution.ToString(),
                        JobID = task.Id.ToString(),
                        Cron = task.Cron.ToString(),
                        sourceLocation = tasklist.Where(x => x.TaskName == task.Id).FirstOrDefault().sourceLocation,
                        DestinationTable = tasklist.Where(x => x.TaskName == task.Id).FirstOrDefault().DestinationTable,
                        TaskDescription = tasklist.Where(x => x.TaskName == task.Id).FirstOrDefault().TaskDescription
                    });
                }

                return Ok(jobdtolist);
            }
            catch
            {
                return BadRequest("Error While Returning Task");
            }
        }
        


        [Authorize]
        [HttpDelete("DeleteTask")]
        public async Task<ActionResult> deletetask(string taskid)
        {
            try
            {
             await _dataProcess.deleteTaskData(taskid);
                RecurringJob.RemoveIfExists(taskid);
                Console.WriteLine("Task Deleted");
                return Ok();
            }
            catch {
                return BadRequest("Deleting Task Failed");
            }
        }

        [Authorize]
        [HttpPost("triggerjob")]

        public async Task<ActionResult> triggerJob(string taskid)
        {
            try
            {

                RecurringJob.TriggerJob(taskid);
                return Ok("Task Triggered");

            }
            catch
            {

                return BadRequest("Failed To Trigger Task");

            }
        }

        

        [Authorize]
        [HttpPost("AssignUsertotask")]
        public async Task<ActionResult> assignUser(string taskid, string username)
        {
            try
            {
                await _authHandlingDao.assignUserToTask(taskid, username);
                return Ok();

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);

            }



        }
        
    }
}
