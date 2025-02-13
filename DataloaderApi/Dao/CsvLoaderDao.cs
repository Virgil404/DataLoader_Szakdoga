using Microsoft.EntityFrameworkCore;
namespace DataloaderApi.Dao
{
    public class CsvLoaderDao<T>: ICsvLoadDao<T>
    {

        private readonly Applicationcontext _context;
        public CsvLoaderDao(Applicationcontext context)
        {
            _context = context;
        }
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

        public async void deletebasedonTableName(string tableName)
        {
            using var db = _context;


            switch (tableName)
            {

                case "crimeData":
                    db.CrimeData.ExecuteDelete(); break;

                case "customers":
                    db.Customers.ExecuteDelete(); break;
                case "organizations":
                    db.Organizations.ExecuteDelete(); break;
                case "product":
                    db.Products.ExecuteDelete(); break;
                default:
                    break;
            }


        }

    }
}
