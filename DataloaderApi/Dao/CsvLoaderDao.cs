using DataloaderApi.DataRead;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using DataloaderApi.Dao.Interfaces;
namespace DataloaderApi.Dao
{
    public class CsvLoaderDao<T>: ICsvLoadDao<T>
    {

        private readonly Applicationcontext _context;
        private readonly string _connectionString;

        public CsvLoaderDao(Applicationcontext context, IConfiguration configuration)
        {
            _context = context;
            _connectionString = configuration.GetConnectionString("dataloaderConnection");
        }

        /*
        public async void insertdataWithoutDelete(List<T> dataModelList)
        {
            using var db = _context;

            //  deletebasedonTableName(tableName);

            foreach (var item in dataModelList)
            {

                

             db.Add(item);

            }



            db.SaveChanges();

        }
        */

        public async void insertdataWithDelete(List<T> dataModelList, string tableName)
        {
            using var db = _context;

             deletebasedonTableName(tableName);


          //  db.AddRangeAsync(dataModelList);

            
            foreach (var item in dataModelList)
            {

               db.Add(item);

            }
           

                  db.SaveChanges() ;
        }



        public async Task insertWithSQLBULK(List<T> dataModelList, string table)
        {
            using var db = _context;

             deletebasedonTableName(table);

            var datatable = dataModelList.ToDataTable();

            int batchSize = 10000;
            int totalRows = datatable.Rows.Count;
            int batches = (int)Math.Ceiling((double)totalRows / batchSize);

            Console.WriteLine($"Total rows: {totalRows}, Batch size: {batchSize}, Total batches: {batches}");

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.Default, transaction))
                        {
                            sqlBulkCopy.DestinationTableName = table;
                            sqlBulkCopy.BatchSize = batchSize;

                           

                            for (int i = 0; i < batches; i++)
                            {
                                int startRow = i * batchSize;
                                int rowsInBatch = Math.Min(batchSize, totalRows - startRow);

                                DataTable batchTable = datatable.AsEnumerable()
                                                                .Skip(startRow)
                                                                .Take(rowsInBatch)
                                                                .CopyToDataTable();

                                Console.WriteLine($"Inserting batch {i + 1}/{batches} ({rowsInBatch} rows)");

                                await sqlBulkCopy.WriteToServerAsync(batchTable);
                            }

                            transaction.Commit();
                            Console.WriteLine("All batches committed successfully.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Problem during insertion: {ex}");
                        transaction.Rollback();
                    }
                }
            }
        }




        public async void deletebasedonTableName(string tableName)
        {
            using var db = _context;


            switch (tableName)
            {

                case "CrimeData":
                    db.CrimeData.ExecuteDelete(); break;

                case "Customers":
                    db.Customers.ExecuteDelete(); break;
                case "Organizations":
                    db.Organizations.ExecuteDelete(); break;
                case "Products":
                    db.Products.ExecuteDelete(); break;
                default:
                    break;
            }


        }

    }
}
