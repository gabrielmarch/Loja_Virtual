
using Loja_Virtual.data;
using Loja_Virtual.model;
using Loja_Virtual.services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Loja_Virtual.controll
{
    public class PedidoController
    {
        public void FinalizarCompra(int clienteId)
        {
            if (!FinalizacaoCompraService.CompraPreparada)
                throw new InvalidOperationException(
                    "Não existe uma compra preparada.");

            if (clienteId <= 0)
                throw new InvalidOperationException(
                    "Cliente não identificado.");

            using (var contexto = new LojaVirtualDbContext())
            using (var transacao =
                   contexto.Database.BeginTransaction())
            {
                try
                {
                    // Verifica se o cliente existe
                    var cliente = contexto.Clientes
                        .FirstOrDefault(c => c.Id == clienteId);

                    if (cliente == null)
                        throw new Exception(
                            "Cliente não encontrado.");

                    // Recupera os dados da compra
                    var itens = FinalizacaoCompraService.Itens;

                    decimal subtotal = 0m;

                    // Confere os produtos no banco
                    foreach (var item in itens)
                    {
                        if (item.Quantidade <= 0)
                            throw new Exception(
                                "Quantidade de produto inválida.");

                        var produto = contexto.Produtos
                            .FirstOrDefault(p => p.Id == item.Id);

                        if (produto == null)
                            throw new Exception(
                                $"Produto {item.Nome} não encontrado.");

                        if (item.Preco < 0)
                            throw new Exception(
                                "Preço de produto inválido.");

                        subtotal += item.Subtotal;
                    }

                    // Valida o cupom, caso exista
                    int? cupomId =
                        FinalizacaoCompraService.CupomId;

                    decimal percentual =
                        FinalizacaoCompraService.PercentualDesconto;

                    if (cupomId.HasValue)
                    {
                        var cupom = contexto.cupoms
                            .FirstOrDefault(c => c.Id == cupomId.Value);

                        if (cupom == null ||
                            !cupom.ativo ||
                            cupom.DataValidade <= DateTime.Now)
                        {
                            throw new Exception(
                                "O cupom não está mais válido.");
                        }

                        percentual = cupom.Desconto;
                    }

                    decimal desconto = Math.Round(
                        subtotal * percentual / 100m, 2);

                    decimal total = subtotal - desconto;

                    if (total < 0)
                        throw new Exception(
                            "Valor total inválido.");

                    // CRIA O PEDIDO
                    Pedido pedido = new Pedido
                    {
                        ClienteId = clienteId,
                        IdCupomId = cupomId,
                        DataPedido = DateTime.Now,
                        Subtotal = subtotal,
                        Desconto = desconto,
                        Status = "Pago"
                    };

                    contexto.Pedidos.Add(pedido);

                    // Salva para obter o ID do pedido
                    contexto.SaveChanges();

                    // CRIA OS ITENS DO PEDIDO
                    foreach (var item in itens)
                    {
                        ItemPedido itemPedido = new ItemPedido
                        {
                            PedidoId = pedido.Id,
                            ProdutoId = item.Id,
                            ServicoId = null,
                            Quantidade = item.Quantidade,
                            PrecoUnitario = item.Preco,
                            Subtotal = item.Subtotal
                        };

                        contexto.itemPedidos.Add(itemPedido);
                    }

                    contexto.SaveChanges();

                    // REGISTRA O PAGAMENTO
                    Pagamento pagamento = new Pagamento
                    {
                        PedidoId = pedido.Id,
                        MetodoPagamento = "Cartão de Crédito/Débito",
                        Valor = total,
                        DataPagamento = DateTime.Now,
                        Status = "Pago"
                    };

                    contexto.Pagamentos.Add(pagamento);

                    contexto.SaveChanges();

                    // Confirma todas as gravações
                    transacao.Commit();
                }
                catch
                {
                    // Desfaz todas as gravações em caso de erro
                    transacao.Rollback();

                    throw;
                }
            }
        }
    }
}
