using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PReMaSys.Migrations
{
    public partial class AddToCart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AddToCart",
                schema: "prms",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RewardsInformationId = table.Column<int>(type: "int", nullable: false),
                    RewardImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RewardName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    RewardDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RewardPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AddToCart", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_AddToCart_Rewards_RewardsInformationId",
                        column: x => x.RewardsInformationId,
                        principalSchema: "prms",
                        principalTable: "Rewards",
                        principalColumn: "RewardsInformationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AddToCart_Users_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "prms",
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AddToCart_ApplicationUserId",
                schema: "prms",
                table: "AddToCart",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AddToCart_RewardsInformationId",
                schema: "prms",
                table: "AddToCart",
                column: "RewardsInformationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AddToCart",
                schema: "prms");
        }
    }
}
