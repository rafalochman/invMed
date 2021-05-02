using Microsoft.EntityFrameworkCore.Migrations;

namespace invMed.Data.Migrations
{
    public partial class addRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "60fa5064-16c1-4936-825d-bb7ebe6fec9f", "f8624421-5eb1-438c-8cb8-6049e41054f4", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0cd05c13-fe91-4ad5-85da-d0ac99fcd7d5", "e8baa849-dae1-441f-8295-9793c3d3f976", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ed813621-b9f8-4557-b1a5-18381cb29832", "6d8bc9d0-858c-4a09-b837-f54056776d9b", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0cd05c13-fe91-4ad5-85da-d0ac99fcd7d5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60fa5064-16c1-4936-825d-bb7ebe6fec9f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ed813621-b9f8-4557-b1a5-18381cb29832");
        }
    }
}
