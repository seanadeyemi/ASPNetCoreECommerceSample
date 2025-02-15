using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPNetCoreECommerceSample.Migrations
{
    public partial class MakeParentCategoryIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "5b590125-d61e-4957-aaff-dbcfa8cb8ba8");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3fce01b5-b2b2-4e83-9bd0-f4571ab56fa4", "AQAAAAEAACcQAAAAEFFDXR+ebMNq1vBq6WmdbmPr88TPRvWVYtvFluGvNHO0i7EaaZP54Uqlia8NjNgEGw==", "3c1fd514-792c-4ed3-8a7b-d53cf63ed1fb" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "f76d1df4-f97e-44e0-83e0-748cd6f432d4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c235ec6c-4066-409c-8122-fb822b592bd2", "AQAAAAEAACcQAAAAENKjjOMZwEMVCulyyLIOcbSWpyFMq7dQVSh6zLnW41+D5dIGuujIcz9QF1dK4iIzkg==", "6d7fc6f5-f316-4d4d-895c-fa4fd77ed6ac" });
        }
    }
}
