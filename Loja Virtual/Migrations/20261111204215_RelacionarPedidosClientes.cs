using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loja_Virtual.Migrations
{
    /// <inheritdoc />
    public partial class RelacionarPedidosClientes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_itemPedidos_pedidos_idPedidoId",
                table: "itemPedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_itemPedidos_produtos_idProdutoId",
                table: "itemPedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_pedidos_Clientes_idClienteId",
                table: "pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_pedidos_cupoms_idCupomId",
                table: "pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_produtos_fornecedores_idFornecedorId",
                table: "produtos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_produtos",
                table: "produtos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_pedidos",
                table: "pedidos");

            migrationBuilder.RenameTable(
                name: "produtos",
                newName: "Produtos");

            migrationBuilder.RenameTable(
                name: "pedidos",
                newName: "Pedidos");

            migrationBuilder.RenameIndex(
                name: "IX_produtos_idFornecedorId",
                table: "Produtos",
                newName: "IX_Produtos_idFornecedorId");

            migrationBuilder.RenameColumn(
                name: "idClienteId",
                table: "Pedidos",
                newName: "ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_pedidos_idCupomId",
                table: "Pedidos",
                newName: "IX_Pedidos_idCupomId");

            migrationBuilder.RenameIndex(
                name: "IX_pedidos_idClienteId",
                table: "Pedidos",
                newName: "IX_Pedidos_ClienteId");


            migrationBuilder.AddPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_itemPedidos_Pedidos_idPedidoId",
                table: "itemPedidos",
                column: "idPedidoId",
                principalTable: "Pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_itemPedidos_Produtos_idProdutoId",
                table: "itemPedidos",
                column: "idProdutoId",
                principalTable: "Produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_Clientes_ClienteId",
                table: "Pedidos",
                column: "ClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_cupoms_idCupomId",
                table: "Pedidos",
                column: "idCupomId",
                principalTable: "cupoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Produtos_fornecedores_idFornecedorId",
                table: "Produtos",
                column: "idFornecedorId",
                principalTable: "fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_itemPedidos_Pedidos_idPedidoId",
                table: "itemPedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_itemPedidos_Produtos_idProdutoId",
                table: "itemPedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_Clientes_ClienteId",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_cupoms_idCupomId",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_Produtos_fornecedores_idFornecedorId",
                table: "Produtos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Produtos",
                table: "Produtos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pedidos",
                table: "Pedidos");


            migrationBuilder.RenameTable(
                name: "Produtos",
                newName: "produtos");

            migrationBuilder.RenameTable(
                name: "Pedidos",
                newName: "pedidos");

            migrationBuilder.RenameIndex(
                name: "IX_Produtos_idFornecedorId",
                table: "produtos",
                newName: "IX_produtos_idFornecedorId");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "pedidos",
                newName: "idClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_idCupomId",
                table: "pedidos",
                newName: "IX_pedidos_idCupomId");

            migrationBuilder.RenameIndex(
                name: "IX_Pedidos_ClienteId",
                table: "pedidos",
                newName: "IX_pedidos_idClienteId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_produtos",
                table: "produtos",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_pedidos",
                table: "pedidos",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_itemPedidos_pedidos_idPedidoId",
                table: "itemPedidos",
                column: "idPedidoId",
                principalTable: "pedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_itemPedidos_produtos_idProdutoId",
                table: "itemPedidos",
                column: "idProdutoId",
                principalTable: "produtos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_pedidos_Clientes_idClienteId",
                table: "pedidos",
                column: "idClienteId",
                principalTable: "Clientes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_pedidos_cupoms_idCupomId",
                table: "pedidos",
                column: "idCupomId",
                principalTable: "cupoms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_produtos_fornecedores_idFornecedorId",
                table: "produtos",
                column: "idFornecedorId",
                principalTable: "fornecedores",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
