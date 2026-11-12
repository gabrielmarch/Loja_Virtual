using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loja_Virtual.Migrations
{
    /// <inheritdoc />
    public partial class AjustesRelacionamentosBanco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_itemPedidos_Produtos_ProdutoId",
                table: "itemPedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_itemPedidos_Servicos_ServicoId",
                table: "itemPedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_itemPedidos_pedidos_PedidoId",
                table: "itemPedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagamentos_pedidos_PedidoId",
                table: "Pagamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_pedidos_Clientes_idClienteId",
                table: "pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_pedidos_Cupons_CupomId",
                table: "pedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pedidos",
                table: "pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pagamentos_PedidoId",
                table: "Pagamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_itemPedidos",
                table: "itemPedidos");

            migrationBuilder.RenameTable(
                name: "pedidos",
                newName: "Pedidos");

            migrationBuilder.RenameTable(
                name: "itemPedidos",
                newName: "ItensPedido");

            migrationBuilder.RenameColumn(
                name: "idClienteId",
                table: "Pedidos",
                newName: "ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_pedidos_CupomId",
                table: "Pedidos",
                newName: "IX_Pedidos_CupomId");

            migrationBuilder.RenameIndex(
                name: "IX_pedidos_idClienteId",
                table: "Pedidos",
                newName: "IX_Pedidos_ClienteId");

            migrationBuilder.RenameColumn(
                name: "Discriminator",
                table: "Pagamentos",
                newName: "TipoPagamento");

            migrationBuilder.RenameIndex(
                name: "IX_itemPedidos_ServicoId",
                table: "ItensPedido",
                newName: "IX_ItensPedido_ServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_itemPedidos_ProdutoId",
                table: "ItensPedido",
                newName: "IX_ItensPedido_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_itemPedidos_PedidoId",
                table: "ItensPedido",
                newName: "IX_ItensPedido_PedidoId");

            migrationBuilder.AlterColumn<string>(
                name: "TipoServico",
                table: "Servicos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "DescricaoServico",
                table: "Servicos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Pedidos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "numeroCartao",
                table: "Pagamentos",
                type: "nvarchar(19)",
                maxLength: 19,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "linhaDigitavel",
                table: "Pagamentos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "bandeira",
                table: "Pagamentos",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Pagamentos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "NomeTitular",
                table: "Pagamentos",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CodigoBarras",
                table: "Pagamentos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos",
                column: "idPedido");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ItensPedido",
                table: "ItensPedido",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_PedidoId",
                table: "Pagamentos",
                column: "PedidoId",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_ItensPedido_ProdutoOuServico",
                table: "ItensPedido",
                sql: "([ProdutoId] IS NOT NULL AND [ServicoId] IS NULL) OR ([ProdutoId] IS NULL AND [ServicoId] IS NOT NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ItensPedido_Quantidade",
                table: "ItensPedido",
                sql: "Quantidade > 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_ItensPedido_Valores",
                table: "ItensPedido",
                sql: "PrecoUnitario >= 0 AND Subtotal >= 0");

            migrationBuilder.AddForeignKey(
                name: "FK_ItensPedido_Pedidos_PedidoId",
                table: "ItensPedido",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "idPedido",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensPedido_Produtos_ProdutoId",
                table: "ItensPedido",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "IdProduto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ItensPedido_Servicos_ServicoId",
                table: "ItensPedido",
                column: "ServicoId",
                principalTable: "Servicos",
                principalColumn: "IdServico",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagamentos_Pedidos_PedidoId",
                table: "Pagamentos",
                column: "PedidoId",
                principalTable: "Pedidos",
                principalColumn: "idPedido",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Clientes_ClienteId",
                table: "Pedidos",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Cupons_CupomId",
                table: "Pedidos",
                column: "CupomId",
                principalTable: "Cupons",
                principalColumn: "idCupom",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ItensPedido_Pedidos_PedidoId",
                table: "ItensPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensPedido_Produtos_ProdutoId",
                table: "ItensPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_ItensPedido_Servicos_ServicoId",
                table: "ItensPedido");

            migrationBuilder.DropForeignKey(
                name: "FK_Pagamentos_Pedidos_PedidoId",
                table: "Pagamentos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Clientes_ClienteId",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Cupons_CupomId",
                table: "Pedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pagamentos_PedidoId",
                table: "Pagamentos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ItensPedido",
                table: "ItensPedido");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ItensPedido_ProdutoOuServico",
                table: "ItensPedido");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ItensPedido_Quantidade",
                table: "ItensPedido");

            migrationBuilder.DropCheckConstraint(
                name: "CK_ItensPedido_Valores",
                table: "ItensPedido");

            migrationBuilder.RenameTable(
                name: "Pedidos",
                newName: "pedidos");

            migrationBuilder.RenameTable(
                name: "ItensPedido",
                newName: "itemPedidos");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "pedidos",
                newName: "idClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_CupomId",
                table: "pedidos",
                newName: "IX_pedidos_CupomId");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_ClienteId",
                table: "pedidos",
                newName: "IX_pedidos_idClienteId");

            migrationBuilder.RenameColumn(
                name: "TipoPagamento",
                table: "Pagamentos",
                newName: "Discriminator");

            migrationBuilder.RenameIndex(
                name: "IX_ItensPedido_ServicoId",
                table: "itemPedidos",
                newName: "IX_itemPedidos_ServicoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensPedido_ProdutoId",
                table: "itemPedidos",
                newName: "IX_itemPedidos_ProdutoId");

            migrationBuilder.RenameIndex(
                name: "IX_ItensPedido_PedidoId",
                table: "itemPedidos",
                newName: "IX_itemPedidos_PedidoId");

            migrationBuilder.AlterColumn<string>(
                name: "TipoServico",
                table: "Servicos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "DescricaoServico",
                table: "Servicos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "pedidos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "numeroCartao",
                table: "Pagamentos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(19)",
                oldMaxLength: 19,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "linhaDigitavel",
                table: "Pagamentos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "bandeira",
                table: "Pagamentos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Pagamentos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "NomeTitular",
                table: "Pagamentos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CodigoBarras",
                table: "Pagamentos",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_pedidos",
                table: "pedidos",
                column: "idPedido");

            migrationBuilder.AddPrimaryKey(
                name: "PK_itemPedidos",
                table: "itemPedidos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Pagamentos_PedidoId",
                table: "Pagamentos",
                column: "PedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_itemPedidos_Produtos_ProdutoId",
                table: "itemPedidos",
                column: "ProdutoId",
                principalTable: "Produtos",
                principalColumn: "IdProduto");

            migrationBuilder.AddForeignKey(
                name: "FK_itemPedidos_Servicos_ServicoId",
                table: "itemPedidos",
                column: "ServicoId",
                principalTable: "Servicos",
                principalColumn: "IdServico");

            migrationBuilder.AddForeignKey(
                name: "FK_itemPedidos_pedidos_PedidoId",
                table: "itemPedidos",
                column: "PedidoId",
                principalTable: "pedidos",
                principalColumn: "idPedido",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pagamentos_pedidos_PedidoId",
                table: "Pagamentos",
                column: "PedidoId",
                principalTable: "pedidos",
                principalColumn: "idPedido",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_pedidos_Clientes_idClienteId",
                table: "pedidos",
                column: "idClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_pedidos_Cupons_CupomId",
                table: "pedidos",
                column: "CupomId",
                principalTable: "Cupons",
                principalColumn: "idCupom");
        }
    }
}
