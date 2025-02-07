using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;

namespace DataloaderApi.DataRead
{
    public class DataReader<T>
    {

        public  List<T> readDataNewFile(string folder, string delimiter, bool hasheader)
        {

            var config =  new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = delimiter,
                HasHeaderRecord = hasheader
            };
            var filepath = new DirectoryInfo(folder).GetFiles().OrderByDescending(d => d.LastWriteTime).First().FullName;

            using (var reader = new StreamReader(filepath))
            using (var csv = new CsvReader(reader, config))
            {
                var records =  csv.GetRecords<T>().ToList();

                return records;
            }

        }



    }
}
