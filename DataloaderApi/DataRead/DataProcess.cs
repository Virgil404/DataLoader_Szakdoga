using System.Data;
using System.Reflection;
using DataloaderApi;
using DataloaderApi.Dao.Interfaces;
using DataloaderApi.Data;
using DataloaderApi.DataRead;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

public class DataProcess
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IdentityContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    private static readonly Dictionary<string, Type> ModelMap = new Dictionary<string, Type>
    {
        { "CrimeData", typeof(CrimeData) },
        { "Customers", typeof(Customers) },
        { "Organizations", typeof(Organizations) },
        { "Products", typeof(Product) },
    };

    public DataProcess(IServiceProvider serviceProvider, IdentityContext context, UserManager<ApplicationUser> userManager)
    {
        _serviceProvider = serviceProvider;
        _context = context;
        _userManager = userManager;
    }


    public async Task InsertToTaskData(string jobname, string filepath , string tablename, ApplicationUser user )
    {
        
        var taskdata = new TaskData
        {
            TaskName = jobname,
            sourceLocation = filepath,
            DestinationTable = tablename,
            AssignedUsers = new List<ApplicationUser> { user },
            TaskDescription ="Test"
            
        };

       await _context.TaskData.AddAsync(taskdata);

      await  _context.SaveChangesAsync();
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

    public async Task  readAndInsert(string filepath, string delimiter, bool hasheader, string tableName)
    {
        try { 
        
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
        catch  { throw  ; }
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
}
