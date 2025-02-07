using CsvHelper.Configuration.Attributes;
using Microsoft.EntityFrameworkCore;

namespace DataloaderApi.Data
{
    public class Product
    {
        [Ignore]
       public  int Id { get; set; }

        [Name("Product_Code")]
        public  string? ProductCode { get; set; }

        [Name ("Warehouse")]
       public  string? Warehouse {  get; set; }

        [Name("Product_Category")]
        public  string? ProductCategory { get; set; }
        [Name("Date")]
        DateTime? CreatedDate { get; set; }
        [Name("Order_Demand")]
         public  int OrderDemand { get; set; }


    }
}
