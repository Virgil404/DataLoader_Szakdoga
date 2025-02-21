using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataloaderApi.Migrations
{
    /// <inheritdoc />
    public partial class UserTablewithSeedFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: "Admin",
                column: "Password",
                value: "$2a$11$0WZaQI.t68SRP7sBnBKFYe5mKMgLuLDQ2CyI5FnZWhBFVO/xRO6qS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserID",
                keyValue: "Admin",
                column: "Password",
                value: "$2a$11$PnY7LFHU6Gsi9WC3IPPv1OPRQF0vvRExLMOh1dxfUS.XhKvsZsLn.");
        }
    }
}
