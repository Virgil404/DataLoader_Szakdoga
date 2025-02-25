﻿using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;
using Dataloader.Api.DTO;
using DataLoader.Services.InterFaces;

namespace DataLoader.Services
{
    public class TaskSchedulerService : ITaskSchedulerService
    {
        private readonly HttpClient httpClient;

        public TaskSchedulerService(HttpClient httpClient) 
        {
            this.httpClient = httpClient;
        }
        /*
        public async Task CreateTask(string name, string _cron, string folder, string _delimiter, bool _hasheader, string tablename)

        {
            try
            {

                var postBody = new
                {
                    cron = _cron,
                    jobname = name,
                    filePath = folder,
                    delimiter = _delimiter,
                    hasheader = _hasheader,
                    tableName = tablename
                };
                var postBodyJson = JsonSerializer.Serialize(postBody);
                var content = new StringContent(postBodyJson, Encoding.UTF8, "application/json");

                using var response = await httpClient.PostAsync("api/Hangfire/CreateTask", content);

                Console.WriteLine("response "+response);
                if (!response.IsSuccessStatusCode)
                {
                    string error = response.ReasonPhrase;

                    throw new Exception(error);

                }

            }
            catch (Exception) { throw; }
        }
        */

        public async Task CreateTask(string name, string _cron, string folder, string _delimiter, bool _hasheader, string tablename)
        {
            try
            {
                var url = $"api/Hangfire/CreateTask?cron={Uri.EscapeDataString(_cron)}&jobname={Uri.EscapeDataString(name)}&filePath={Uri.EscapeDataString(folder)}&delimiter={Uri.EscapeDataString(_delimiter)}&hasheader={_hasheader}&tableName={Uri.EscapeDataString(tablename)}";

                var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                using var response = await httpClient.PostAsync(url, content);

                Console.WriteLine("Response: " + response);
                if (!response.IsSuccessStatusCode)
                {
                    string error = response.ReasonPhrase;
                    throw new Exception(error);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task DeleteTask(string jobID)
        {

            try
            {


                using var response = await httpClient.DeleteAsync($"api/Hangfire/DeleteTask?taskid={jobID}");

                if (!response.IsSuccessStatusCode)
                {

                    string error = response.ReasonPhrase;
                    throw new Exception(error);


                }

            }
            catch (Exception) { throw; }
        }

        public async Task<List<TaskDTO>> GetTasks()
        {
            try
            {
                var Joblist = await httpClient.GetFromJsonAsync<List<TaskDTO>>("api/Hangfire/getRecurrningJobs");

                return Joblist;

            }
            catch (Exception){ 
                
                throw;

            }
        }


        /*
        public async Task TriggerTask(string jobID)
        {

            try { 

            var postBody = new
            {
               
                taskid = jobID,
            };
            var postBodyJson = JsonSerializer.Serialize(postBody);
            var content = new StringContent(postBodyJson, Encoding.UTF8, "application/json");
            using var response = await httpClient.PostAsync($"api/Hangfire/triggerjob",content);


            if (!response.IsSuccessStatusCode)
                {

                string error = response.ReasonPhrase;
                throw new Exception(error);


                }
            }
            catch(Exception) { throw; }

        }
        */

        public async Task TriggerTask(string jobID)
        {
            try
            {
                var url = $"api/Hangfire/triggerjob?taskid={Uri.EscapeDataString(jobID)}";

                var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

                using var response = await httpClient.PostAsync(url, content);

                Console.WriteLine("Response: " + response);
                if (!response.IsSuccessStatusCode)
                {
                    string error = response.ReasonPhrase;
                    throw new Exception(error);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
