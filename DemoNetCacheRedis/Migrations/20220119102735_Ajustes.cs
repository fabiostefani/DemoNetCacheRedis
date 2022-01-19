using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemoNetCacheRedis.Migrations
{
    public partial class Ajustes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Produto",
                newName: "IdProduto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdProduto",
                table: "Produto",
                newName: "Id");
        }
    }
}
