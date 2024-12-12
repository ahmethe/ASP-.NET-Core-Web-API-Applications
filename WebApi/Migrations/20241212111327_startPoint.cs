using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

/* 
<summary>
    Migration çalıştırabilmek için MicrosoftEntitiyFrameworkCore.Tools 
    paketine ihtiyacımız var. Bunu package manager console yardımıyla kuruyoruz.
    Bu paket yardımı ile komut setine sahip oluyoruz. (Add-Migration, Update-Database)
    Migration alabilmek için ise ana projemizde MicrosoftEntityFrameworkCore.Design paketi olmalı.
    EfCore hangi uygulama üzerinde çalışacaksa orada bu pakete ihtiyacımız var.
</summary>
*/

namespace WebApi.Migrations
{
    /// <inheritdoc />
    public partial class startPoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
