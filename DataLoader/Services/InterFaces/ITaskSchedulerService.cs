using Dataloader.Api.DTO;

namespace DataLoader.Services.InterFaces
{
    public interface ITaskSchedulerService
    {

        public Task<List<TaskDTO>> GetTasks();

        public Task<List<DetailedTaskDTO>> GetTasksAssignedToUser();

        public Task CreateTask(string name , string cron , string folder, string delimiter , bool hasheader , string tablename, string description );

        public Task DeleteTask(string jobID);

        public Task TriggerTask(string jobID);

        public Task AssignUser(string jobID, string username);
    }
}
