using System.Data;
using DataloaderApi;
using DataloaderApi.Dao.Interfaces;
using DataloaderApi.Data;
using DataloaderApi.DataRead;
using DataloaderApi.Extension.Services;
using DataloaderApi.Extension.Services.Interface;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public class DataProcess
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IdentityContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IEmaliSenderService  _emailSender;

    private static readonly Dictionary<string, Type> ModelMap = new Dictionary<string, Type>
    {
        { "CrimeData", typeof(CrimeData) },
        { "Customers", typeof(Customers) },
        { "Organizations", typeof(Organizations) },
        { "Products", typeof(Product) },
    };

    public DataProcess(IServiceProvider serviceProvider, IdentityContext context, UserManager<ApplicationUser> userManager, IEmaliSenderService emailSender)
    {
        _serviceProvider = serviceProvider;
        _context = context;
        _userManager = userManager;
        _emailSender = emailSender;
    }

    public async Task<ICollection<ApplicationUser>> GetAssignedusers(string jobname)
    {

        var taskdata = _context.TaskData
            .Include(t => t.AssignedUsers)
            .FirstOrDefault(x => x.TaskName == jobname);
        if (taskdata != null)
        {
            var users = taskdata.AssignedUsers;

            return users;
        }
        return null;
    }

    public async Task InsertToTaskData(string jobname, string filepath, string tablename, ApplicationUser user, string description)
    {

        var taskdata = new TaskData
        {
            TaskName = jobname,
            sourceLocation = filepath,
            DestinationTable = tablename,
            AssignedUsers = new List<ApplicationUser> { user },
            TaskDescription = description

        };

        await _context.TaskData.AddAsync(taskdata);

        await _context.SaveChangesAsync();
    }

    public async Task deleteTaskData(string jobname)
    {
        var taskdata = _context.TaskData.Where(x => x.TaskName == jobname).FirstOrDefault();
        if (taskdata != null)
        {
            _context.TaskData.Remove(taskdata);
            await _context.SaveChangesAsync();
        }
    }

    public async Task UpdateTaskData(string jobname, string filepath, string tablename, string description)
    {
        var taskdata = _context.TaskData.Where(x => x.TaskName == jobname).FirstOrDefault();
        if (taskdata != null)
        {
            taskdata.TaskName = jobname;
            taskdata.sourceLocation = filepath;
            taskdata.DestinationTable = tablename;
            taskdata.TaskDescription = description;
            await _context.SaveChangesAsync();
        }
    }


    public async Task readAndInsert(string filepath, string delimiter, bool hasheader, string tableName)
    {
        try
        {
            //var sendmail = applicationUser== null ? false : true;

            if (!ModelMap.TryGetValue(tableName, out Type modelType))
            {
                Console.WriteLine($"Error: No model found for table '{tableName}'");
                throw new Exception($"Error: No model found for table '{tableName}'");
            }



            dynamic dataReader = Activator.CreateInstance(typeof(DataReader<>).MakeGenericType(modelType));
            var dataList = dataReader.readDataNewFile(filepath, delimiter, hasheader);

            var dbHandler = _serviceProvider.GetRequiredService(typeof(ICsvLoadDao<>).MakeGenericType(modelType));

            await ((dynamic)dbHandler).insertWithSQLBULK(dataList, tableName);

            Console.WriteLine("Insert complete.");


        }
        catch { throw; }
    }

    public async Task ExecuteJobAndNotify(string jobname, string filepath, string delimiter, bool hasheader, string tableName)
    {
        var usersAssignedToTask = await GetAssignedusers(jobname);
        List<string> recipients = usersAssignedToTask.Select(x => x.Email).ToList();
        try
        {
            await readAndInsert(filepath, delimiter, hasheader, tableName);
            Console.WriteLine($"{jobname} executed successfully.");

          //  var usersAssignedToTask = await GetAssignedusers(jobname);
           // List<string> recipients =  usersAssignedToTask.Select(x => x.Email).ToList();

            // Send email notification
            await _emailSender.SendEmail(
                recipients,
                $"Job {jobname} Completed",
                $"The scheduled job '{jobname}' has been successfully completed at {DateTime.Now}."
            );

            Console.WriteLine($"Email notification sent for {jobname}.");
        }
        catch (Exception ex)
        {

            await _emailSender.SendEmail(
              recipients,
              $"Job {jobname} Not Completed",
              $"The scheduled job '{jobname}' has not Completed at  {DateTime.Now}. error:{ex.Message}"
          );
            Console.WriteLine($"Error while executing job {jobname}: {ex.Message}");
        }
    }



    public void readAndInsertWithoutDelete(string filepath, string delimiter, bool hasheader, string tableName)
    {

        if (!ModelMap.TryGetValue(tableName.ToLower(), out Type modelType))
        {
            Console.WriteLine($"Error: No model found for table '{tableName}'");
            return;
        }

        dynamic dataReader = Activator.CreateInstance(typeof(DataReader<>).MakeGenericType(modelType));
        var dataList = dataReader.readDataNewFile(filepath, delimiter, hasheader);

        var dbHandler = _serviceProvider.GetRequiredService(typeof(ICsvLoadDao<>).MakeGenericType(modelType));

        ((dynamic)dbHandler).insertdataWithoutDelete(dataList);

        Console.WriteLine("Insert complete.");

    }

    public List<TaskData> getTaskData(ApplicationUser? currentuser)
    {
        try
        {
            var tasks = _context.TaskData.Where(x => x.AssignedUsers.Contains(currentuser)).ToList();
            return tasks;


        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return null;
        }
    }
    public List<TaskData> getAllTaskData()
    {
        try
        {
            var tasks = _context.TaskData.ToList();
            return tasks;
        }



        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            return null;
        }

    }

}
