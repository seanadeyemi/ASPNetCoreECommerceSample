using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPNetCoreECommerceSample.Migrations
{
    public partial class MakeParentCategoryIdNullable2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParentCategoryId",
                table: "Categories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2c5e174e-3b0e-446f-86af-483d56fd7210",
                column: "ConcurrencyStamp",
                value: "d2ec1e2f-d267-4f55-b41f-73a2183d1a13");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "707aa1f8-34ba-4b00-a82e-c58d2f4daa09", "AQAAAAEAACcQAAAAEPo2EBaar/iBUI7r45wicbB//rnqMm/c2nDvN+6vIi2OtBXK5jizkniKwqqkPIXHOg==", "29695c83-666d-448a-a3ae-327e6e0ca29f" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ParentCategoryId",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
