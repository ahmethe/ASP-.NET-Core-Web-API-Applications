using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesToDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0e465656-9b8a-4477-8aaa-2c3e42a5fc16", null, "Editor", "EDITOR" },
                    { "d051f8a3-cf49-4f66-a288-1d46698f8d3b", null, "User", "USER" },
                    { "d9025152-1316-474d-bdb2-adf93a8ce9be", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0e465656-9b8a-4477-8aaa-2c3e42a5fc16");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d051f8a3-cf49-4f66-a288-1d46698f8d3b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d9025152-1316-474d-bdb2-adf93a8ce9be");
        }
    }
}
