using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventStock.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProjectLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockUser");

            migrationBuilder.DropTable(
                name: "UserStockRoles");

            migrationBuilder.AddColumn<int>(
                name: "StockId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_StockId",
                table: "AspNetUsers",
                column: "StockId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Stocks_StockId",
                table: "AspNetUsers",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Stocks_StockId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_StockId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "StockId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "StockUser",
                columns: table => new
                {
                    StocksId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockUser", x => new { x.StocksId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_StockUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockUser_Stocks_StocksId",
                        column: x => x.StocksId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserStockRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    StockId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserStockRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserStockRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStockRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserStockRoles_Stocks_StockId",
                        column: x => x.StockId,
                        principalTable: "Stocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockUser_UsersId",
                table: "StockUser",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStockRoles_RoleId",
                table: "UserStockRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStockRoles_StockId",
                table: "UserStockRoles",
                column: "StockId");

            migrationBuilder.CreateIndex(
                name: "IX_UserStockRoles_UserId",
                table: "UserStockRoles",
                column: "UserId");
        }
    }
}
