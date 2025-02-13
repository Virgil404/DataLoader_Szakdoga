using Dataloader.Api.DTO;
using DataLoader.Services.InterFaces;
using Microsoft.AspNetCore.Components;

namespace DataLoader.Pages
{
    public class JobsBlazor:ComponentBase
    {
        [Inject]
      public   ITaskSchedulerService taskSchedulerService { get; set; }

        public string Jobname { get; set; }
        public string cron {  get; set; }
        public string filepath { get; set; }

        public string delimiter { get; set; }
        public string tablename { get; set; }

        public bool alert { get; set; }
        public bool hasheader { get; set; }
       public TaskDTO task { get; set;}
      public List<TaskDTO> tasks { get; set; }

        
        

        protected override async Task OnInitializedAsync()
        {
          tasks = await taskSchedulerService.GetTasks();

        }



        public async Task createTask()
        {
            if (Jobname == null || cron == null || filepath == null || delimiter == null || tablename == null) { 
            
                return ;    
            }

           await taskSchedulerService.CreateTask(Jobname, cron, filepath, delimiter, hasheader, tablename);
            alert = true;
        }


        public void setAlert()
        {

            alert = false;
        }



    }
}
