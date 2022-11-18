using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PReMaSys.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Purchase",
                schema: "prms",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AddToCartCartId = table.Column<int>(type: "int", nullable: true),
                    EmployeeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RewardImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RewardName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RewardPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPayment = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Stat = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchase", x => x.PurchaseId);
                    table.ForeignKey(
                        name: "FK_Purchase_AddToCart_AddToCartCartId",
                        column: x => x.AddToCartCartId,
                        principalSchema: "prms",
                        principalTable: "AddToCart",
                        principalColumn: "CartId");
                    table.ForeignKey(
                        name: "FK_Purchase_Users_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "prms",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_AddToCartCartId",
                schema: "prms",
                table: "Purchase",
                column: "AddToCartCartId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchase_ApplicationUserId",
                schema: "prms",
                table: "Purchase",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Purchase",
                schema: "prms");
        }
    }
}