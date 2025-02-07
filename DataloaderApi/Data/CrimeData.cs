using System.ComponentModel.DataAnnotations;
using CsvHelper.Configuration.Attributes;

namespace DataloaderApi.Data
{
    public class CrimeData
    {
        [Key]
        [Name("DR_NO")]
        public int DR_NO { get; set; }

        [Name("Date Rptd")]
        public DateTime DateRptd { get; set; }

        [Name("DATE OCC")]
        public DateTime DATEOCC { get; set; }

        [Name("TIME OCC")]
        public int TIMEOCC { get; set; }

        [Name("AREA")]
        public int AREA { get; set; }

        [Name("AREA NAME")]
        public string? AREANAME { get; set; }

        [Name("Rpt Dist No")]
        public int RptDistNo { get; set; }

        [Name("Part1-2")]
        public int Part1_2 { get; set; }

        [Name("Crm Cd")]
        public int CrmCd { get; set; }
        [Name("Crm Cd Desc")]
        public string? CrmCdDesc { get; set; }
        [Name("Mocodes")]
        public string? Mocodes { get; set; }
        [Name("Vict Age")]
        public int VictAge { get; set; }
        [Name("Vict Sex")]
        public string ?VictSex { get; set; }
        [Name("Vict Descent")]
        public string ?VictDescent { get; set; }
        [Name("Premis Cd")]
        public int PremisCd { get; set; }
        [Name("Premis Cd")]
        public string? PremisDesc { get; set; }
        [Name("Weapon Used Cd")]
        public string? WeaponUsedCd { get; set; }
        [Name("Weapon Desc")]
        public string? WeaponDesc { get; set; }
        [Name("Status")]
        public string Status { get; set; }

        [Name("Status Desc")]
        public string? StatusDesc { get; set; }

        [Name("Crm Cd 1")]
        public int CrmCd1 { get; set; }

        [Name("Crm Cd 2")]
        public int? CrmCd2 { get; set; }

        [Name("Crm Cd 3")]
        public string? CrmCd3 { get; set; }

        [Name("Crm Cd 4")]
        public string? CrmCd4 { get; set; }

        [Name("LOCATION")]
        public string? LOCATION { get; set; }

        [Name("Cross Street")]
        public string? CrossStreet { get; set; }

        [Name("LAT")]
        public double LAT { get; set; }

        [Name("LON")]
        public double LON { get; set; }
    }
}
