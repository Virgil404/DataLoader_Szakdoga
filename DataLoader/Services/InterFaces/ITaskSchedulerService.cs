﻿using Dataloader.Api.DTO;

namespace DataLoader.Services.InterFaces
{
    public interface ITaskSchedulerService
    {

        public Task<List<TaskDTO>> GetTasks();

        public Task CreateTask(string name , string cron , string folder, string delimiter , bool hasheader , string tablename );

        public Task DeleteTask(string jobID);

        public Task TriggerTask(string jobID);

    }
}
