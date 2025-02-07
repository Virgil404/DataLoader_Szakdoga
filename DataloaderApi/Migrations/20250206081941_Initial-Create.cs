using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataloaderApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CrimeData",
                columns: table => new
                {
                    DR_NO = table.Column<int>(type: "int", nullable: false),
                       // .Annotation("SqlServer:Identity", "1, 1"),
                    DateRptd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DATEOCC = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TIMEOCC = table.Column<int>(type: "int", nullable: false),
                    AREA = table.Column<int>(type: "int", nullable: false),
                    AREANAME = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RptDistNo = table.Column<int>(type: "int", nullable: false),
                    Part1_2 = table.Column<int>(type: "int", nullable: false),
                    CrmCd = table.Column<int>(type: "int", nullable: false),
                    CrmCdDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mocodes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VictAge = table.Column<int>(type: "int", nullable: false),
                    VictSex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VictDescent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PremisCd = table.Column<int>(type: "int", nullable: false),
                    PremisDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeaponUsedCd = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeaponDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CrmCd1 = table.Column<int>(type: "int", nullable: false),
                    CrmCd2 = table.Column<int>(type: "int", nullable: true),
                    CrmCd3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CrmCd4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LOCATION = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CrossStreet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LAT = table.Column<double>(type: "float", nullable: false),
                    LON = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrimeData", x => x.DR_NO);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                       // .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerID = table.Column<long>(type: "bigint", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneOne = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneTwo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscriptionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false),
                       // .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Founded = table.Column<int>(type: "int", nullable: false),
                    Industry = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberOfEmployes = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Warehouse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderDemand = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tables", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CrimeData");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Tables");
        }
    }
}
