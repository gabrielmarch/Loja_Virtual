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
        public async Task<int> RegistrarPedidoAsync()
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
                Status = "AguardandoPagamento"
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

            decimal percentual = CarrinhoServiço.PercentualDesconto;

            if (percentual != 0m && percentual != 10m)
            {
                throw new InvalidOperationException(
                    "Desconto inválido.");
            }

            decimal desconto = Math.Round(
                subtotal * percentual / 100m,
                2,
                MidpointRounding.AwayFromZero);

            pedido.Subtotal = subtotal;
            pedido.Desconto = desconto;

            banco.pedidos.Add(pedido);

            await banco.SaveChangesAsync();

            return pedido.idPedido;
        }
    }
}
