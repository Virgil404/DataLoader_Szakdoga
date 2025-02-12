using DataloaderApi.Dao;
using DataloaderApi.Data;
using DataloaderApi.DataRead;
using Microsoft.Extensions.DependencyInjection;

public class DataProcess
{
    private readonly IServiceProvider _serviceProvider;

    private static readonly Dictionary<string, Type> ModelMap = new Dictionary<string, Type>
    {
        { "crimeData", typeof(CrimeData) },
        { "customers", typeof(Customers) },
        { "organizations", typeof(Organizations) },
        { "product", typeof(Product) },
    };

    public DataProcess(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void readAndInsert(string filepath, string delimiter, bool hasheader, string tableName)
    {
        if (!ModelMap.TryGetValue(tableName, out Type modelType))
        {
            Console.WriteLine($"Error: No model found for table '{tableName}'");
            return;
        }

        dynamic dataReader = Activator.CreateInstance(typeof(DataReader<>).MakeGenericType(modelType));
        var dataList = dataReader.readDataNewFile(filepath, delimiter, hasheader);

        var dbHandler = _serviceProvider.GetRequiredService(typeof(ICsvLoadDao<>).MakeGenericType(modelType));

        ((dynamic)dbHandler).insertdataWithDelete(dataList,tableName);

        Console.WriteLine("Insert complete.");
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
