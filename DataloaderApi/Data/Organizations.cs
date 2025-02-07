using CsvHelper.Configuration.Attributes;

namespace DataloaderApi.Data
{
    public class Organizations
    {

        [Name("Index")]
        public int ID { get; set; }

        [Name("Organization Id")]
        public string? OrganizationID { get; set; }

        [Name("Name")]
        public string? OrganizationName { get; set; }

        [Name("Website")]
        public string? Website { get; set; }

        [Name("Country")]
        public string ? Country { get; set; }


        [Name("Description")]
        public string? Description { get; set; }

        [Name("Founded")]
        public int Founded { get; set; }

        [Name("Industry")]
        public string? Industry { get; set; }

        [Name("Number of employees")]
        public int NumberOfEmployes { get; set; }

    }
}
