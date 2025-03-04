using Dataloader.Api.DTO;
using DataLoader.Services;
using DataLoader.Services.InterFaces;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Radzen;

namespace DataLoader.Pages
{
    public class JobsBlazor:ComponentBase
    {
        [Inject]
      public   ITaskSchedulerService taskSchedulerService { get; set; }
        [Inject]
        public NotificationService NotificationService { get; set; }
        public string Jobname { get; set; }
        public string cron {  get; set; }
        public string filepath { get; set; }

        public string delimiter { get; set; }
        public string tablename { get; set; }
        public string description { get; set; }

        //public bool alert { get; set; }

        public string crontext { get; set; }

        public bool retriggerlert { get; set; }
        public  bool BadAlert {  get; set; } 
        public bool UseCron { get; set; }
       public TaskDTO task { get; set;}
      public List<TaskDTO> tasks { get; set; }
      
       public DetailedTaskDTO detailedTask { get; set; }
        public List<DetailedTaskDTO> detailedTasks { get; set; }


        public Dictionary<string, string> CronMap = new Dictionary<string, string>
       {
           {"every minute","* * * * *" },
           {"every hour" , "0 * * * *"},
           {"every day", "0 0 * * *"},
           {"every week", "0 0 * * 0" },
           {"every month", "0 0 1 * *" },
           {"every year","0 0 1 1 *" }
       };


        protected override async Task OnInitializedAsync()
        {
          tasks = await taskSchedulerService.GetTasks();
          detailedTasks= await taskSchedulerService.GetTasksAssignedToUser();
        }



        public async Task createTask()
        {
            if (!UseCron)
            {

                if (crontext.IsNullOrEmpty())
                {
                    NotificationService.Notify(new NotificationMessage
                    {
                        Severity = NotificationSeverity.Warning,
                        Summary = "Task Not Created",
                        Detail = $"You have to select an interval or select the use Cron option",
                        Duration = 6000

                    });
                    return;
                }

                if (CronMap.TryGetValue(crontext, out string cronstring))
                {
                    cron=cronstring;

                }

                
            }

            if (Jobname == null || cron == null || filepath == null || delimiter == null || tablename == null|| description==null) {

                NotificationService.Notify(new NotificationMessage
                { Severity = NotificationSeverity.Warning, Summary = "Task Not Created", 
                    Detail = $"Please fill out all the fields", Duration = 6000 });

                // BadAlert = true;

                return ;    
            }



           await taskSchedulerService.CreateTask(Jobname, cron, filepath, delimiter, true, tablename,description);
            NotificationService.Notify(new NotificationMessage
            { Severity = NotificationSeverity.Success, Summary = "Task Created Successfully", Detail = $"{Jobname} created successfully", Duration = 6000 });
            //alert = true;
        }


        public async Task RefreshList()
        {
            tasks = await taskSchedulerService.GetTasks();
            detailedTasks = await taskSchedulerService.GetTasksAssignedToUser();
            StateHasChanged(); 
        }

        public async Task Delete(string jobid)
        {

            await taskSchedulerService.DeleteTask(jobid);
            tasks = await taskSchedulerService.GetTasks();
            StateHasChanged();
        }

        public async Task Trigger (string jobid)
        {

            await taskSchedulerService.TriggerTask(jobid);
            tasks = await taskSchedulerService.GetTasks();
            StateHasChanged();

            NotificationService.Notify(new NotificationMessage
            { Severity = NotificationSeverity.Success, Summary = "Task Triggered", Detail = $"{jobid} triggered succesfully", Duration = 6000 });
            // retriggerlert= true;

        }


    }
}
