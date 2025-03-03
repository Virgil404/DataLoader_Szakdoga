using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataloaderApi.Migrations.Identity
{
    /// <inheritdoc />
    public partial class userTaskJunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TaskData",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TaskDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sourceLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DestinationTable = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    isActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskData", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaskDataUserJunction",
                columns: table => new
                {
                    AssignedUsersId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TasksId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskDataUserJunction", x => new { x.AssignedUsersId, x.TasksId });
                    table.ForeignKey(
                        name: "FK_TaskDataUserJunction_AspNetUsers_AssignedUsersId",
                        column: x => x.AssignedUsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskDataUserJunction_TaskData_TasksId",
                        column: x => x.TasksId,
                        principalTable: "TaskData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaskDataUserJunction_TasksId",
                table: "TaskDataUserJunction",
                column: "TasksId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskDataUserJunction");

            migrationBuilder.DropTable(
                name: "TaskData");
        }
    }
}
