using System;
using System.Collections.Generic;
using System.Text;
using Loja_Virtual.data;
using Loja_Virtual.model;
using Loja_Virtual.services;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Loja_Virtual.controll
{
    public class PagamentoController
    {
        public async Task ConfirmarBoletoSimuladoAsync(int pedidoId)
        {
            if (SessaoUsuario.ClienteId == null)
            {
                throw new InvalidOperationException(
                    "Faça login para confirmar o pagamento.");
            }

            int clienteId = SessaoUsuario.ClienteId.Value;

            using var banco = new LojaVirtualDbContext();
            await using var transacao =
                await banco.Database.BeginTransactionAsync(
                    IsolationLevel.ReadCommitted);

            try
            {
                int alterados = await banco.pedidos
                    .Where(p =>
                        p.idPedido == pedidoId &&
                        p.ClienteId == clienteId &&
                        p.Status == "AguardandoPagamento")
                    .ExecuteUpdateAsync(setters => setters
                        .SetProperty(p => p.Status, "Pago"));

                if (alterados != 1)
                {
                    throw new InvalidOperationException(
                        "Pedido não encontrado, não pertence a você " +
                        "ou não está aguardando pagamento.");
                }

                var pedido = await banco.pedidos
                    .AsNoTracking()
                    .Include(p => p.Itens)
                    .FirstAsync(p => p.idPedido == pedidoId);

                if (pedido.Itens.Count == 0)
                {
                    throw new InvalidOperationException(
                        "O pedido não possui itens.");
                }

                foreach (var item in pedido.Itens.OrderBy(
                    i => i.ProdutoId))
                {
                    if (item.ProdutoId == null ||
                        item.Quantidade <= 0)
                    {
                        throw new InvalidOperationException(
                            "O pedido possui um item inválido.");
                    }

                    int produtoId = item.ProdutoId.Value;
                    int quantidade = item.Quantidade;
                    int atualizado = await banco.produtos
                        .Where(p =>
                            p.IdProduto == produtoId &&
                            p.EstoqueProduto >= quantidade)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(
                                p => p.EstoqueProduto,
                                p => p.EstoqueProduto - quantidade));

                    if (atualizado != 1)
                    {
                        throw new InvalidOperationException(
                            $"Estoque insuficiente para o produto " +
                            $"{produtoId}. Nenhuma alteração foi salva.");
                    }
                }

                var pagamento = new Boleto
                {
                    PedidoId = pedidoId,
                    ValorPagamento =
                        pedido.Subtotal - pedido.Desconto,
                    DataPagamento = DateTime.Now,
                    DataVencimento = DateTime.Today,
                    Status = "PagoSimulado",
                    CodigoBarras = string.Empty,
                    linhaDigitavel = string.Empty
                };

                banco.Pagamentos.Add(pagamento);

                await banco.SaveChangesAsync();
                await transacao.CommitAsync();
            }
            catch
            {

                await transacao.RollbackAsync();
                throw;
            }
        }


        public async Task<int> RegistrarCompraCartaoSimuladaAsync(
            string nomeTitular, decimal valorEsperado)
        {

            if (SessaoUsuario.ClienteId == null)
            {
                throw new InvalidOperationException(
                    "Você precisa estar conectado para comprar.");
            }

            int clienteId = SessaoUsuario.ClienteId.Value;
            nomeTitular = nomeTitular.Trim();

            if (string.IsNullOrWhiteSpace(nomeTitular) ||
                nomeTitular.Length > 150)
            {
                throw new ArgumentException(
                    "Informe um nome válido para a simulação.");
            }

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
                .OrderBy(i => i.IdProduto)
                .ToList();

            using var banco = new LojaVirtualDbContext();

            await using var transacao =
                await banco.Database.BeginTransactionAsync(
                    IsolationLevel.ReadCommitted);

            try
            {
                bool clienteAtivo = await banco.Clientes
                    .AnyAsync(c =>
                        c.Id == clienteId && c.Ativo);

                if (!clienteAtivo)
                {
                    throw new InvalidOperationException(
                        "Cliente não encontrado ou inativo.");
                }

                var pedido = new Pedido
                {
                    ClienteId = clienteId,
                    DataPedido = DateTime.Now,
                    Status = "Pago",
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
                        .AsNoTracking()
                        .FirstOrDefaultAsync(p =>
                            p.IdProduto == item.IdProduto);

                    if (produto == null)
                    {
                        throw new InvalidOperationException(
                            $"Produto {item.IdProduto} não encontrado.");
                    }

                    int alterados = await banco.produtos
                        .Where(p =>
                            p.IdProduto == item.IdProduto &&
                            p.EstoqueProduto >= item.Quantidade)
                        .ExecuteUpdateAsync(setters => setters
                            .SetProperty(
                                p => p.EstoqueProduto,
                                p => p.EstoqueProduto - item.Quantidade));

                    if (alterados != 1)
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

                decimal percentualDesconto = 0m;

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

                    percentualDesconto = cupom.Desconto;
                }


                decimal desconto = Math.Round(
                    subtotal * percentualDesconto / 100m,
                    2,
                    MidpointRounding.AwayFromZero);

                decimal total = subtotal - desconto;
                if (total != valorEsperado)
                {
                    throw new InvalidOperationException(
                        "O valor da compra foi alterado. " +
                        "Volte ao carrinho e confira os preços antes de confirmar.");
                }

                if (total <= 0)
                {
                    throw new InvalidOperationException(
                        "O valor da compra deve ser positivo.");
                }

                pedido.Subtotal = subtotal;
                pedido.Desconto = desconto;
                pedido.Pagamentos.Add(new Cartao
                {
                    NomeTitular = nomeTitular,
                    bandeira = "Simulado",
                    numeroCartao = string.Empty,
                    parcelas = 1,
                    ValorPagamento = total,
                    DataPagamento = DateTime.Now,
                    Status = "PagoSimulado"
                });
                banco.pedidos.Add(pedido);

                await banco.SaveChangesAsync();
                await transacao.CommitAsync();

                return pedido.idPedido;
            }
            catch
            {
                await transacao.RollbackAsync();
                throw;
            }
        }


    }



}
