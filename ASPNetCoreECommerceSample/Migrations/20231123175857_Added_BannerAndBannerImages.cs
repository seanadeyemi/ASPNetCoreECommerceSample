using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPNetCoreECommerceSample.Migrations
{
    public partial class Added_BannerAndBannerImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description2 = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BannerImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannerImages_Banners_BannerId",
                        column: x => x.BannerId,
                        principalTable: "Banners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Banners",
                columns: new[] { "Id", "Description1", "Description2", "Title" },
                values: new object[] { 1, "Exquisite Suits", "Nice Jackets", "Buchi Summer Collection" });

            migrationBuilder.InsertData(
                table: "Banners",
                columns: new[] { "Id", "Description1", "Description2", "Title" },
                values: new object[] { 2, "Blazers", "Sneakers", "Victor Designer wears" });

            migrationBuilder.InsertData(
                table: "BannerImages",
                columns: new[] { "Id", "BannerId", "ImagePath" },
                values: new object[,]
                {
                    { 1, 1, "" },
                    { 2, 1, "" },
                    { 3, 2, "" },
                    { 4, 2, "" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BannerImages_BannerId",
                table: "BannerImages",
                column: "BannerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BannerImages");

            migrationBuilder.DropTable(
                name: "Banners");
        }
    }
}
