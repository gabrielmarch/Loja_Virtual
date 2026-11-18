using System;
using System.Collections.Generic;
using System.Text;
using Loja_Virtual.data;
using Loja_Virtual.model;
using Loja_Virtual.services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Loja_Virtual.controll
{
    public class PedidoController
    {
        public async Task<int> RegistrarPedidoAsync(decimal valorEsperado)
        {
            if (!SessaoUsuario.EstaAutenticado ||
                SessaoUsuario.ClienteId == null)
            {
                throw new InvalidOperationException(
                    "É necessário realizar login para comprar.");
            }

            int clienteId = SessaoUsuario.ClienteId.Value;

            if (CarrinhoServiço.Itens.Count == 0)
            {
                throw new InvalidOperationException(
                    "O carrinho está vazio.");
            }

            var itensCarrinho = CarrinhoServiço.Itens
                .Select(i => new
                {
                    i.IdProduto,
                    i.Quantidade
                })
                .ToList();

            using var banco = new LojaVirtualDbContext();

            bool clienteExiste = await banco.Clientes
                .AnyAsync(c => c.Id == clienteId && c.Ativo);

            if (!clienteExiste)
            {
                throw new InvalidOperationException(
                    "Cliente não encontrado ou inativo.");
            }

            var pedido = new Pedido
            {
                ClienteId = clienteId,
                DataPedido = DateTime.Now,
                Status = "AguardandoPagamento",
                EtapaPedido = "PedidoFeito"
            };

            decimal subtotal = 0m;

            foreach (var item in itensCarrinho)
            {
                if (item.Quantidade <= 0)
                {
                    throw new InvalidOperationException(
                        "Quantidade inválida no carrinho.");
                }

                Produto? produto = await banco.produtos
                    .FirstOrDefaultAsync(p =>
                        p.IdProduto == item.IdProduto);

                if (produto == null)
                {
                    throw new InvalidOperationException(
                        $"Produto {item.IdProduto} não encontrado.");
                }

                if (produto.EstoqueProduto < item.Quantidade)
                {
                    throw new InvalidOperationException(
                        $"Estoque insuficiente para {produto.NomeProduto}.");
                }

                decimal subtotalItem =
                    produto.Preco * item.Quantidade;

                pedido.Itens.Add(new ItemPedido
                {
                    ProdutoId = produto.IdProduto,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = produto.Preco,
                    Subtotal = subtotalItem
                });

                subtotal += subtotalItem;
            }

            decimal percentual = 0m;

            string? codigoCupom = CarrinhoServiço.CodigoCupom;

            if (!string.IsNullOrWhiteSpace(codigoCupom))
            {
                Cupom? cupom = await CupomService.BuscarValidoAsync(
                    banco, codigoCupom);

                if (cupom == null)
                {
                    throw new InvalidOperationException(
                        "O cupom aplicado expirou ou foi desativado.");
                }
                pedido.CupomId = cupom.idCupom;
                percentual = cupom.Desconto;
            }

            decimal desconto = Math.Round(
                subtotal * percentual / 100m,
                2,
                MidpointRounding.AwayFromZero);

            decimal total = subtotal - desconto;

            if (total != valorEsperado)
            {
                throw new InvalidOperationException(
                    "O valor da compra mudou. " +
                    "Volte ao carrinho e confira o total.");
            }

            pedido.Subtotal = subtotal;
            pedido.Desconto = desconto;

            banco.pedidos.Add(pedido);

            await banco.SaveChangesAsync();

            return pedido.idPedido;
        }
    }
}
