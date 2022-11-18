using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PReMaSys.Migrations
{
    public partial class TestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesEmployeeRecords_Users_ApplicationUserId",
                schema: "prms",
                table: "SalesEmployeeRecords");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                schema: "prms",
                table: "SalesEmployeeRecords",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateTable(
                name: "SERecord",
                schema: "prms",
                columns: table => new
                {
                    SEmployeeRecordsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SERIdId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    EmployeeNo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeFirstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeLastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeBirthdate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeePoints = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SERecord", x => x.SEmployeeRecordsID);
                    table.ForeignKey(
                        name: "FK_SERecord_Users_SERIdId",
                        column: x => x.SERIdId,
                        principalSchema: "prms",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SERecord_SERIdId",
                schema: "prms",
                table: "SERecord",
                column: "SERIdId");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesEmployeeRecords_Users_ApplicationUserId",
                schema: "prms",
                table: "SalesEmployeeRecords",
                column: "ApplicationUserId",
                principalSchema: "prms",
                principalTable: "Users",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesEmployeeRecords_Users_ApplicationUserId",
                schema: "prms",
                table: "SalesEmployeeRecords");

            migrationBuilder.DropTable(
                name: "SERecord",
                schema: "prms");

            migrationBuilder.AlterColumn<string>(
                name: "ApplicationUserId",
                schema: "prms",
                table: "SalesEmployeeRecords",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesEmployeeRecords_Users_ApplicationUserId",
                schema: "prms",
                table: "SalesEmployeeRecords",
                column: "ApplicationUserId",
                principalSchema: "prms",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
