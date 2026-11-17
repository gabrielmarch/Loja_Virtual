using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loja_Virtual.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarPerfilAcesso : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PerfilAcesso",
                table: "Clientes",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Cliente");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerfilAcesso",
                table: "Clientes");
        }
    }
}
