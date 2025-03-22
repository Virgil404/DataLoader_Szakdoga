using Dataloader.Api.DTO;
using DataLoader.Services;
using DataLoader.Services.InterFaces;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Radzen;
using System.Text.RegularExpressions;

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
        public string userToassign { get; set; }

        //public bool alert { get; set; }

        public string crontext { get; set; }

        public bool retriggerlert { get; set; }
        public  bool BadAlert {  get; set; } 
        public bool UseCron { get; set; }
       public DetailedTaskDTO task { get; set;}
      public List<DetailedTaskDTO> tasks { get; set; }
      
       public DetailedTaskDTO TasksAssignedToCurrentuser { get; set; }
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

            if (!IsCronValid(cron))
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Task Not Created",
                    Detail = $"Invalid Cron Expression",
                    Duration = 6000
                }); 
                return;
            }

            if (!(tablename== "CrimeData"||tablename== "Customers"|| tablename== "Organizations"||tablename== "Products"))
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Task Not Created",
                    Detail = $"Invalid table Name",
                    Duration = 6000
                });
                return;

            }
            if (!isFolderValid(filepath))
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Warning,
                    Summary = "Task Not Created",
                    Detail = $"Invalid Folder Path",
                    Duration = 6000
                });
                return;
            }


            if ( String.IsNullOrEmpty(Jobname) || String.IsNullOrEmpty( cron)  ||String.IsNullOrEmpty(filepath)  || String.IsNullOrEmpty(delimiter) || String.IsNullOrEmpty(tablename)||String.IsNullOrEmpty( description)) {

                NotificationService.Notify(new NotificationMessage
                { Severity = NotificationSeverity.Warning, Summary = "Task Not Created", 
                    Detail = $"Please fill out all the fields", Duration = 6000 });

                // BadAlert = true;

                return ;    
            }

         //   Console.WriteLine("Jobname:" +(Jobname==null));


           await taskSchedulerService.CreateTask(Jobname, cron, filepath, delimiter, true, tablename,description);
            NotificationService.Notify(new NotificationMessage
            { Severity = NotificationSeverity.Success, Summary = "Task Created Successfully", Detail = $"{Jobname} created successfully", Duration = 6000 });
            //alert = true;
        }

        private  bool IsCronValid(string cron)
        {
            if (string.IsNullOrWhiteSpace(cron))
            {
                return false;
            }

            var cronPattern = @"^(\*|([0-5]?\d)(-[0-5]?\d)?(\/[0-5]?\d)?(,[0-5]?\d)*)\s" +     // Minute (0-59)
                              @"(\*|([0-1]?\d|2[0-3])(-([0-1]?\d|2[0-3]))?(\/([0-1]?\d|2[0-3]))?(,([0-1]?\d|2[0-3]))*)\s" + // Hour (0-23)
                              @"(\*|([1-9]|[12]\d|3[01])(-([1-9]|[12]\d|3[01]))?(\/([1-9]|[12]\d|3[01]))?(,([1-9]|[12]\d|3[01]))*)\s" + // Day of Month (1-31)
                              @"(\*|(1[0-2]|0?[1-9])(-([1-9]|1[0-2]))?(\/([1-9]|1[0-2]))?(,(1[0-2]|0?[1-9]))*)\s" + // Month (1-12)
                              @"(\*|([0-6])(-([0-6]))?(\/([0-6]))?(,([0-6]))*)$"; // Day of Week (0-6)

            return Regex.IsMatch(cron, cronPattern, RegexOptions.IgnoreCase);
        }

        private bool isFolderValid(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                return false;
            }
            var folderPattern = @".+(?=\\)";
            ;
            return Regex.IsMatch(folder, folderPattern, RegexOptions.IgnoreCase);
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


        public async Task AssingUserToTask(string username, string taskid)


        {
            try { 
            await taskSchedulerService.AssignUser(taskid, username);

                NotificationService.Notify(new NotificationMessage
                { Severity = NotificationSeverity.Success, Summary = "User Added", Detail = $"{username} Added To task successfully", Duration = 6000 });
                // retriggerlert= true
            }
            catch(Exception e )
            {
                NotificationService.Notify(new NotificationMessage
                { Severity = NotificationSeverity.Warning, Summary = "User Not Added", Detail = e.Message, Duration = 6000 });
            }
        }


        public void FillfieldsifClicked(string _jobid, string _description , string _sourcelocation, string _destinationTable, string _cron)
        {
            Jobname = _jobid;
            description = _description;
            filepath = _sourcelocation;
            tablename = _destinationTable;
            cron = _cron;
            UseCron = true;

        }

    }
}
