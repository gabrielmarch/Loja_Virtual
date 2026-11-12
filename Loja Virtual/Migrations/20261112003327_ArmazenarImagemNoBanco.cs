using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loja_Virtual.Migrations
{
    /// <inheritdoc />
    public partial class ArmazenarImagemNoBanco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaminhoImagem",
                table: "Produtos");

            migrationBuilder.AddColumn<byte[]>(
                name: "ImagemProduto",
                table: "Produtos",
                type: "varbinary(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagemProduto",
                table: "Produtos");

            migrationBuilder.AddColumn<string>(
                name: "CaminhoImagem",
                table: "Produtos",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
