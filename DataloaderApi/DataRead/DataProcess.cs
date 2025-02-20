using System.Data;
using System.Reflection;
using DataloaderApi.Dao;
using DataloaderApi.Data;
using DataloaderApi.DataRead;
using Microsoft.Extensions.DependencyInjection;


public class DataProcess
{
    private readonly IServiceProvider _serviceProvider;

    private static readonly Dictionary<string, Type> ModelMap = new Dictionary<string, Type>
    {
        { "CrimeData", typeof(CrimeData) },
        { "Customers", typeof(Customers) },
        { "Organizations", typeof(Organizations) },
        { "Products", typeof(Product) },
    };

    public DataProcess(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
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


    


}
