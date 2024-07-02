using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPNetCoreECommerceSample.Migrations
{
    public partial class wishlistupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Customers_CustomerId",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_CustomerId",
                table: "Wishlists");

            migrationBuilder.AlterColumn<string>(
                name: "CustomerId",
                table: "Wishlists",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId1",
                table: "Wishlists",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_CustomerId1",
                table: "Wishlists",
                column: "CustomerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Customers_CustomerId1",
                table: "Wishlists",
                column: "CustomerId1",
                principalTable: "Customers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlists_Customers_CustomerId1",
                table: "Wishlists");

            migrationBuilder.DropIndex(
                name: "IX_Wishlists_CustomerId1",
                table: "Wishlists");

            migrationBuilder.DropColumn(
                name: "CustomerId1",
                table: "Wishlists");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "Wishlists",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlists_CustomerId",
                table: "Wishlists",
                column: "CustomerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlists_Customers_CustomerId",
                table: "Wishlists",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");
        }
    }
}
