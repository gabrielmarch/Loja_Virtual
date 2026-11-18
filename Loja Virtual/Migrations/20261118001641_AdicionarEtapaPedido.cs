using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loja_Virtual.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarEtapaPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EtapaPedido",
                table: "pedidos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EtapaPedido",
                table: "pedidos");
        }
    }
}
