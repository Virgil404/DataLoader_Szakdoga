using Microsoft.EntityFrameworkCore;

namespace DataloaderApi.Dao
{
    public interface ICsvLoadDao<T>
    {

        public void insertdataWithoutDelete(List<T> dataModelList)
        {
        }


        public void insertdataWithDelete(List<T> dataModelList,string tableName)
        {
        }


        public void deletebasedonTableName(string tableName)
        {
        }

        public async Task insertWithSQLBULK(List<T> dataModelList, string table) { }
    }
}
